using System;

namespace NewMaster.Core
{
    public sealed class CounterAttackService
    {
        public bool TryClaim(CounterAttackState state, GameState gameState, EconomyService economy)
        {
            if (state == null || gameState == null || economy == null || !state.Available || state.Bounty <= 0)
                return false;

            var bounty = state.Bounty;
            var granted = economy.GrantCoins(gameState, bounty);
            state.Clear();
            gameState.Status.Set(NewMaster.Localization.NewMasterTextKeys.RaidLoot, granted);
            return granted > 0;
        }

        public long Preview(CounterAttackState state) =>
            state != null && state.Available ? Math.Max(0L, state.Bounty) : 0L;
    }
}
