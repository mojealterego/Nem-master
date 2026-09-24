using System;

namespace NewMaster.Core
{
    public sealed class EconomyService
    {
        public bool TrySpendCoins(GameState state, long amount)
        {
            if (state == null || amount < 0 || state.Coins < amount)
                return false;

            state.Coins -= amount;
            return true;
        }

        public long GrantCoins(GameState state, long amount)
        {
            if (state == null || amount <= 0)
                return 0;

            var granted = Math.Min(amount, long.MaxValue - state.Coins);
            if (granted <= 0)
                return 0;

            state.Coins += granted;
            return granted;
        }

        public int GrantEnergy(GameState state, int amount)
        {
            if (state == null || amount <= 0)
                return 0;

            var granted = Math.Min(amount, int.MaxValue - state.Energy);
            if (granted <= 0)
                return 0;

            state.Energy += granted;
            return granted;
        }
    }
}
