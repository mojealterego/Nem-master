using System;

namespace NewMaster.Core
{
    public sealed class WorldBossService
    {
        public bool EnsureBoss(WorldBossState state, string bossId, long maxHealth)
        {
            if (state == null || string.IsNullOrWhiteSpace(bossId) || maxHealth <= 0)
                return false;

            state.Normalize();
            if (string.Equals(state.BossId, bossId, StringComparison.Ordinal) && state.MaxHealth > 0)
                return true;

            state.Initialize(bossId, maxHealth);
            return true;
        }

        public long Attack(WorldBossState state, long damage)
        {
            if (state == null || damage <= 0 || state.Defeated || state.Health <= 0)
                return 0;

            var applied = Math.Min(damage, state.Health);
            state.Health -= applied;
            state.PersonalContribution = state.PersonalContribution > long.MaxValue - applied
                ? long.MaxValue
                : state.PersonalContribution + applied;
            if (state.Health == 0)
                state.Defeated = true;
            return applied;
        }

        public bool TryClaim(WorldBossState state, long reward, EconomyService economy, GameState gameState)
        {
            if (state == null || economy == null || gameState == null || !state.Defeated || state.RewardClaimed || reward <= 0)
                return false;

            economy.GrantCoins(gameState, reward);
            state.RewardClaimed = true;
            return true;
        }
    }
}
