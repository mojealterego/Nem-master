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
        private readonly LocalSaveService saveService = new();
        private readonly WorldRuleService worldRules = new();

        public void SaveProgress()
        {
            EnsureState();
            GameStateMigrations.Normalize(state);
            villageState?.Normalize();
            collectionState?.Normalize();
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
                Publish();

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
                Publish();

            return upgraded;
        }

        public void NotifyStateChanged() => Publish();

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
            var outcome = SpinRules.Resolve(
                slots,
                baseReward,
                world == null ? 1 : world.energyReward);

            state.Slots = new List<string>(slots);
            economy.GrantCoins(state, outcome.Coins);
            economy.GrantEnergy(state, outcome.Energy);
            state.SpinsWon = outcome.Type == SpinOutcomeType.Nothing
                ? state.SpinsWon
                : (state.SpinsWon == int.MaxValue ? int.MaxValue : state.SpinsWon + 1);

            if (outcome.VillageProgress > 0)
            {
                state.CurrentVillageLevel = (int)Math.Min(
                    int.MaxValue,
                    (long)state.CurrentVillageLevel + outcome.VillageProgress);
            }

            if (outcome.VillageProgress > 0)
                state.Status.Set(NewMasterTextKeys.VillageProgress);
            else if (outcome.Coins > 0)
                state.Status.Set(NewMasterTextKeys.CoinsReward, outcome.Coins);
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
            villageState ??= new VillageState();
            collectionState ??= new CollectionState();
        }

        private void Publish() => StateChanged?.Invoke(state);
    }
}
