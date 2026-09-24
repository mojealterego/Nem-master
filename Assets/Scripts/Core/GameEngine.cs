using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NemMaster.Worlds;

namespace NemMaster.Core
{
    public sealed class GameEngine : MonoBehaviour
    {
        [SerializeField] private GameState state = new();
        [SerializeField] private List<WorldDefinition> worlds = new();

        public GameState State => state;
        public event Action<GameState> StateChanged;

        private readonly System.Random rng = new();

        public void Spin()
        {
            if (state.IsSpinning || state.Energy <= 0)
                return;

            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            state.IsSpinning = true;
            state.Energy--;
            Publish();

            yield return new WaitForSeconds(0.8f);

            var symbols = GetCurrentSymbols();
            state.Slots = new List<string>
            {
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)],
                symbols[rng.Next(symbols.Count)]
            };

            var reward = CalculateReward(state.Slots) * GetRewardMultiplier();
            state.Coins += reward;
            state.StatusMessage = reward > 0 ? $"+{reward:N0} monet!" : "Spróbuj ponownie.";
            state.IsSpinning = false;
            Publish();
        }

        public bool UnlockNextWorld()
        {
            var next = worlds.Find(w => w.id == state.CurrentWorldId + 1);
            if (next == null || state.Coins < next.unlockCost)
                return false;

            state.Coins -= next.unlockCost;
            state.CurrentWorldId = next.id;
            state.StatusMessage = $"Odblokowano: {next.worldName}";
            Publish();
            return true;
        }

        public void AddEnergy(int amount)
        {
            state.Energy = Mathf.Max(0, state.Energy + amount);
            Publish();
        }

        private List<string> GetCurrentSymbols()
        {
            var world = worlds.Find(w => w.id == state.CurrentWorldId);
            return world != null && world.symbols.Count > 0
                ? world.symbols
                : new List<string> { "Coin", "Crown", "Chest", "Energy", "Hammer" };
        }

        private int CalculateReward(IReadOnlyList<string> result)
        {
            if (result.Count == 3 && result[0] == result[1] && result[1] == result[2])
                return 1000;

            if (result.Count == 3 && result[0] == "Coin" && result[1] == "Coin")
                return 250;

            return 0;
        }

        private long GetRewardMultiplier()
        {
            var world = worlds.Find(w => w.id == state.CurrentWorldId);
            return world == null ? 1L : Math.Max(1L, Mathf.RoundToInt(world.rewardMultiplier));
        }

        private void Publish() => StateChanged?.Invoke(state);
    }
}
