using NewMaster.Core;

namespace NewMaster.Monetization
{
    public sealed class BattlePassService
    {
        private readonly EconomyService economy = new();

        public int GetUnlockedTier(BattlePassDefinition definition, BattlePassState state)
        {
            if (definition == null || state == null || definition.tiers == null)
                return 0;

            var tier = 0;
            for (var i = 0; i < definition.tiers.Count; i++)
            {
                var entry = definition.tiers[i];
                if (entry != null && entry.level > tier &&
                    state.Experience >= entry.experienceRequired)
                    tier = entry.level;
            }

            return tier;
        }

        public bool TryClaimNext(
            BattlePassDefinition definition,
            BattlePassState state,
            GameState gameState)
        {
            if (definition == null || state == null || gameState == null ||
                state.SeasonId != definition.seasonId ||
                definition.tiers == null)
                return false;

            var unlocked = GetUnlockedTier(definition, state);
            var next = FindTier(definition, state.ClaimedTier + 1);
            if (next == null || next.level > unlocked)
                return false;

            economy.GrantCoins(gameState, next.coinReward);
            economy.GrantEnergy(gameState, next.energyReward);
            state.ClaimedTier = next.level;
            return true;
        }

        private static BattlePassDefinition.Tier FindTier(
            BattlePassDefinition definition,
            int level)
        {
            for (var i = 0; i < definition.tiers.Count; i++)
            {
                var tier = definition.tiers[i];
                if (tier != null && tier.level == level)
                    return tier;
            }

            return null;
        }
    }
}
