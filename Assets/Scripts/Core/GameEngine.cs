using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewMaster.Localization;
using NewMaster.Collections;
using NewMaster.Progression;
using NewMaster.Village;
using NewMaster.Worlds;

namespace NewMaster.Core
{
    public sealed class GameEngine : MonoBehaviour
    {
        [SerializeField] private GameState state = new();
        [SerializeField] private WorldCatalog worldCatalog;
        [SerializeField] private VillageState villageState = new();
        [SerializeField] private CollectionState collectionState = new();
        [SerializeField, Min(0f)] private float spinDuration = 0.8f;
        [SerializeField, Min(5f)] private float autoSaveIntervalSeconds = 30f;
        [SerializeField, Min(0)] private int battlePassXpPerSpin = 10;
        [SerializeField, Min(0)] private int masteryXpPerSpin = 25;
        [SerializeField, Min(0)] private int streakBonusAtThree = 250;
        [SerializeField, Min(0)] private int streakBonusAtFive = 750;
        [SerializeField, Min(0)] private int streakBonusAtTen = 2500;
        [SerializeField, Min(1)] private long worldBossBaseHealth = 10000;
        [SerializeField, Min(1)] private long worldBossDamagePerAttack = 250;
        [SerializeField, Min(1)] private long worldBossBaseReward = 5000;

        private Coroutine autoSaveCoroutine;
        private WorldCatalog runtimeWorldCatalog;

        public GameState State => state;
        public VillageState VillageState => villageState;
        public CollectionState CollectionState => collectionState;
        public event Action<GameState> StateChanged;

        private readonly System.Random rng = new();
        private readonly EconomyService economy = new();
        private readonly ProgressionService progression = new();
        private readonly VillageService village = new();
        private readonly VillageDefenseService villageDefense = new();
        private readonly LocalSaveService saveService = new();
        private readonly WorldRuleService worldRules = new();
        private readonly DailyRewardService dailyRewards = new();
        private readonly BattlePassProgressionService battlePass = new();
        private readonly MasteryService mastery = new();
        private readonly MasteryPerkService masteryPerks = new();
        private readonly LiveOpsProgressService liveOps = new();
        private readonly WorldBossService worldBoss = new();

        public void SaveProgress()
        {
            EnsureState();
            GameStateMigrations.Normalize(state);
            villageState.Normalize();
            collectionState.Normalize();
            saveService.Save(state, GameState.CurrentVersion);
            saveService.SaveVillage(villageState, GameState.CurrentVersion);
            saveService.SaveCollections(collectionState, GameState.CurrentVersion);
        }

        public bool LoadProgress()
        {
            if (!saveService.TryLoad<GameState>(out var loaded))
                return false;

            state = loaded;
            EnsureState();
            GameStateMigrations.Normalize(state);
            state.IsSpinning = false;

            if (!saveService.TryLoadVillage<VillageState>(out var loadedVillage))
                loadedVillage = new VillageState();

            villageState = loadedVillage;
            villageState.Normalize();

            if (!saveService.TryLoadCollections<CollectionState>(out var loadedCollections))
                loadedCollections = new CollectionState();

            collectionState = loadedCollections;
            collectionState.Normalize();
            BeginSessionIfNeeded();
            Publish();
            return true;
        }

        private void Awake()
        {
            EnsureState();

            if (worldCatalog == null)
            {
                runtimeWorldCatalog = WorldCatalog.CreateRuntimeFallback();
                worldCatalog = runtimeWorldCatalog;
            }
        }

        private void Start()
        {
            if (!LoadProgress())
            {
                BeginSessionIfNeeded();
                Publish();
            }

            autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }

        private void OnDestroy()
        {
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
                autoSaveCoroutine = null;
            }

            if (runtimeWorldCatalog != null)
            {
                Destroy(runtimeWorldCatalog);
                runtimeWorldCatalog = null;
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                SaveProgress();
        }

        private void OnApplicationQuit() => SaveProgress();

        public void Spin()
        {
            EnsureState();

            if (state.IsSpinning || state.Energy <= 0)
                return;

            StartCoroutine(SpinRoutine());
        }

        public bool TryUnlockNextWorld()
        {
            EnsureState();
            var unlocked = progression.TryUnlockNextWorld(state, worldCatalog);
            if (unlocked)
                Publish();

            return unlocked;
        }

        public bool TryUpgradeBuilding(BuildingDefinition building)
        {
            EnsureState();
            var upgraded = village.TryUpgrade(state, villageState, building);
            if (upgraded)
            {
                if (state.Session != null && state.Session.BuildingsUpgradedThisSession < int.MaxValue)
                    state.Session.BuildingsUpgradedThisSession++;
                Publish();
            }

            return upgraded;
        }

        public int GetVillageDefenseScore()
        {
            EnsureState();
            villageState.Normalize();
            return villageDefense.GetDefenseScore(villageState);
        }

        public int ApplyRaidDamageToVillage(int damage)
        {
            EnsureState();
            var applied = villageDefense.ApplyRaidDamage(villageState, damage);
            if (applied > 0)
            {
                state.Status.Set(NewMasterTextKeys.VillageRaidDamage, applied);
                Publish();
            }

            return applied;
        }

        public bool TryRepairBuilding(int buildingId, long repairCost)
        {
            EnsureState();
            var repaired = villageDefense.TryRepair(state, villageState, buildingId, repairCost);
            if (repaired)
                Publish();

            return repaired;
        }

        public bool TryAttackWorldBoss()
        {
            EnsureState();
            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);
            if (world == null || string.IsNullOrWhiteSpace(world.bossId) || state.Energy <= 0)
                return false;

            var maxHealth = Math.Max(
                worldBossBaseHealth,
                worldBossBaseHealth + (long)state.CurrentWorldId * 250L);
            worldBoss.EnsureBoss(state.WorldBoss, world.bossId, maxHealth);

            if (state.WorldBoss.Defeated)
                return false;

            state.Energy--;
            var damage = Math.Max(
                1L,
                worldBossDamagePerAttack + (long)state.CurrentWorldId * 10L);
            var applied = worldBoss.Attack(state.WorldBoss, damage);
            if (applied <= 0)
            {
                state.Energy++;
                return false;
            }

            state.Status.Set(
                state.WorldBoss.Defeated
                    ? NewMasterTextKeys.WorldBossDefeated
                    : NewMasterTextKeys.WorldBossAttack,
                applied,
                state.WorldBoss.Health);
            Publish();
            return true;
        }

        public bool TryClaimWorldBossReward()
        {
            EnsureState();
            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);
            if (world == null || string.IsNullOrWhiteSpace(world.bossId))
                return false;

            var reward = Math.Max(
                worldBossBaseReward,
                worldBossBaseReward + (long)state.CurrentWorldId * 500L);
            if (!worldBoss.TryClaim(state.WorldBoss, reward, economy, state))
                return false;

            state.Status.Set(NewMasterTextKeys.WorldBossClaimed, reward);
            Publish();
            return true;
        }

        public void NotifyStateChanged() => Publish();

        public bool TryClaimDailyReward()
        {
            EnsureState();

            var now = DateTimeOffset.UtcNow;
            var reward = dailyRewards.Preview(state, now);
            if (!reward.IsAvailable || !dailyRewards.TryClaim(state, now))
                return false;

            state.Status.Set(
                NewMasterTextKeys.DailyReward,
                reward.Day,
                reward.Coins,
                reward.Energy.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Publish();
            return true;
        }

        public void AddEnergy(int amount)
        {
            EnsureState();

            if (amount >= 0)
                economy.GrantEnergy(state, amount);
            else
                state.Energy = (int)Math.Max(0L, (long)state.Energy + amount);

            Publish();
        }

        private IEnumerator AutoSaveRoutine()
        {
            var waitSeconds = Mathf.Max(5f, autoSaveIntervalSeconds);
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(waitSeconds);

                if (state != null && !state.IsSpinning)
                    SaveProgress();
            }
        }

        private IEnumerator SpinRoutine()
        {
            state.IsSpinning = true;
            state.Energy--;
            Publish();

            yield return new WaitForSeconds(Mathf.Max(0f, spinDuration));

            var symbols = GetCurrentSymbols();
            var slots = new List<string>
            {
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)]
            };

            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);
            var baseReward = world == null
                ? 100L
                : Math.Max(100L, worldRules.ApplyRewardMultiplier(world, world.baseSpinReward));
            var outcome = SpinRules.Resolve(slots, baseReward, world == null ? 1 : world.energyReward);

            state.Slots = new List<string>(slots);

            var masteryRank = mastery.GetRank(state.Mastery.Experience);
            var masteryAdjustedCoins = masteryPerks.ApplyCoinBonus(outcome.Coins, masteryRank);
            var masteryEnergyBonus = outcome.Energy > 0
                ? masteryPerks.GetEnergyRewardBonus(masteryRank)
                : 0;
            var grantedCoins = economy.GrantCoins(state, masteryAdjustedCoins);
            economy.GrantEnergy(state, AddClamped(outcome.Energy, masteryEnergyBonus));
            battlePass.AddExperience(state.BattlePass, battlePassXpPerSpin);
            liveOps.AddProgress(state.LiveOps, "spin.50", 1, 50);
            liveOps.AddProgress(state.LiveOps, "spin.250", 1, 250);
            if (outcome.Type == SpinOutcomeType.Jackpot)
                liveOps.AddProgress(state.LiveOps, "jackpot.10", 1, 10);

            var masteryXp = masteryXpPerSpin;
            if (outcome.Type == SpinOutcomeType.Jackpot)
                masteryXp = AddClamped(masteryXp, 250);
            else if (outcome.VillageProgress > 0)
                masteryXp = AddClamped(masteryXp, 75);
            mastery.AddExperience(state.Mastery, masteryXp);

            if (outcome.Type != SpinOutcomeType.Nothing && state.SpinsWon < int.MaxValue)
                state.SpinsWon++;

            if (outcome.Type != SpinOutcomeType.Nothing)
            {
                if (state.SpinStreak < int.MaxValue)
                    state.SpinStreak++;

                if (state.SpinStreak > state.BestSpinStreak)
                    state.BestSpinStreak = state.SpinStreak;

                var streakBonus = GetStreakBonus(state.SpinStreak);
                if (streakBonus > 0)
                    economy.GrantCoins(state, streakBonus);
            }
            else
            {
                state.SpinStreak = 0;
            }

            if (state.Session != null)
            {
                if (state.Session.SpinsThisSession < int.MaxValue)
                    state.Session.SpinsThisSession++;

                if (grantedCoins > 0)
                    state.Session.CoinsEarnedThisSession = AddClamped(
                        state.Session.CoinsEarnedThisSession,
                        grantedCoins);
            }

            if (outcome.VillageProgress > 0)
            {
                state.CurrentVillageLevel = (int)Math.Min(
                    int.MaxValue,
                    (long)state.CurrentVillageLevel + outcome.VillageProgress);
            }

            if (outcome.Type == SpinOutcomeType.Jackpot)
                state.Status.Set(NewMasterTextKeys.JackpotReward, grantedCoins);
            else if (outcome.VillageProgress > 0)
                state.Status.Set(NewMasterTextKeys.VillageProgress);
            else if (outcome.Coins > 0)
                state.Status.Set(NewMasterTextKeys.CoinsReward, grantedCoins);
            else if (outcome.Energy > 0)
                state.Status.Set(NewMasterTextKeys.EnergyReward, outcome.Energy);
            else
                state.Status.Set(NewMasterTextKeys.NoReward);

            state.IsSpinning = false;
            Publish();
        }

        private List<string> GetCurrentSymbols()
        {
            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);

            if (world != null && world.symbols != null && world.symbols.Count > 0)
                return world.symbols;

            return new List<string> { "Coin", "Crown", "Chest", "Energy", "Hammer" };
        }

        private void EnsureState()
        {
            state ??= new GameState();
            state.BattlePass ??= new BattlePassState();
            state.BattlePass.Normalize(int.MaxValue);
            state.Mastery ??= new MasteryState();
            state.Mastery.Normalize();
            state.LiveOps ??= new LiveOpsProgressState();
            state.LiveOps.Normalize();
            state.Session ??= new GameSessionState();
            state.WorldBoss ??= new WorldBossState();
            state.CounterAttack ??= new CounterAttackState();
            villageState ??= new VillageState();
            collectionState ??= new CollectionState();
        }

        private void BeginSessionIfNeeded()
        {
            EnsureState();
            if (state.Session.SessionId > 0)
                return;

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            state.Session.Reset(now, now);
        }

        private int GetStreakBonus(int streak)
        {
            if (streak == 10)
                return streakBonusAtTen;
            if (streak == 5)
                return streakBonusAtFive;
            if (streak == 3)
                return streakBonusAtThree;
            return 0;
        }

        private static int AddClamped(int current, long amount)
        {
            if (amount <= 0)
                return current;

            return (int)Math.Min(
                int.MaxValue,
                (long)current + amount);
        }

        private void Publish() => StateChanged?.Invoke(state);
    }
}
