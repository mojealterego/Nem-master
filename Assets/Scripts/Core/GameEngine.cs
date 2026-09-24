using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField] private float spinDuration = 0.8f;

        public GameState State => state;
        public VillageState VillageState => villageState;
        public event Action<GameState> StateChanged;

        public void SaveProgress()
        {
            saveService.Save(state, GameState.CurrentVersion);
            saveService.SaveVillage(villageState, GameState.CurrentVersion);
        }

        public bool LoadProgress()
        {
            if (!saveService.TryLoad<GameState>(out var loaded))
                return false;

            state = loaded;
            state.IsSpinning = false;

            if (!saveService.TryLoadVillage<VillageState>(out var loadedVillage))
                loadedVillage = new VillageState();

            villageState = loadedVillage;
            Publish();
            return true;
        }

        private readonly System.Random rng = new();
        private readonly ProgressionService progression = new();
        private readonly VillageService village = new();
        private readonly LocalSaveService saveService = new();

        public void Spin()
        {
            if (state.IsSpinning || state.Energy <= 0)
                return;

            StartCoroutine(SpinRoutine());
        }

        public bool TryUnlockNextWorld()
        {
            var unlocked = progression.TryUnlockNextWorld(state, worldCatalog);
            if (unlocked)
                Publish();

            return unlocked;
        }

        public bool TryUpgradeBuilding(BuildingDefinition building)
        {
            var upgraded = village.TryUpgrade(state, villageState, building);
            if (upgraded)
                Publish();

            return upgraded;
        }

        public void NotifyStateChanged() => Publish();

        public void AddEnergy(int amount)
        {
            state.Energy = Mathf.Max(0, state.Energy + amount);
            Publish();
        }

        private IEnumerator SpinRoutine()
        {
            state.IsSpinning = true;
            state.Energy--;
            Publish();

            yield return new WaitForSeconds(spinDuration);

            var symbols = GetCurrentSymbols();
            var slots = new List<string>
            {
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)]
            };

            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);
            var baseReward = world == null ? 100L : Math.Max(100L, (long)Math.Round(world.baseSpinReward * world.rewardMultiplier));
            var outcome = SpinRules.Resolve(slots, baseReward, world == null ? 1 : world.energyReward);

            state.Slots = new List<string>(slots);
            state.Coins += outcome.Coins;
            state.Energy += outcome.Energy;
            state.SpinsWon += outcome.Type == SpinOutcomeType.Nothing ? 0 : 1;

            if (outcome.VillageProgress > 0)
            {
                state.CurrentVillageLevel += outcome.VillageProgress;
                state.StatusMessage = "Postęp wioski +1";
            }
            else if (outcome.Coins > 0)
            {
                state.StatusMessage = $"+{outcome.Coins:N0} monet";
            }
            else if (outcome.Energy > 0)
            {
                state.StatusMessage = $"+{outcome.Energy} energii";
            }
            else
            {
                state.StatusMessage = "Brak nagrody. Następny obrót może zmienić wszystko.";
            }

            state.IsSpinning = false;
            Publish();
        }

        private List<string> GetCurrentSymbols()
        {
            var world = worldCatalog == null ? null : worldCatalog.Find(state.CurrentWorldId);

            if (world != null && world.symbols.Count > 0)
                return world.symbols;

            return new List<string> { "Coin", "Crown", "Chest", "Energy", "Hammer" };
        }

        private void Publish() => StateChanged?.Invoke(state);
    }
}
