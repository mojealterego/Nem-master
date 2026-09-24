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

            state.Coins += amount;
            return amount;
        }

        public int GrantEnergy(GameState state, int amount)
        {
            if (state == null || amount <= 0)
                return 0;

            state.Energy += amount;
            return amount;
        }
    }
}
