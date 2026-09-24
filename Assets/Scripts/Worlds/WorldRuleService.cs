using System;

namespace NewMaster.Worlds
{
    public sealed class WorldRuleService
    {
        public bool IsSeasonal(WorldDefinition world) =>
            world != null && !string.IsNullOrWhiteSpace(world.seasonalTag);

        public bool HasBoss(WorldDefinition world) =>
            world != null && !string.IsNullOrWhiteSpace(world.bossId);

        public long ApplyRewardMultiplier(WorldDefinition world, long baseReward)
        {
            if (world == null || baseReward <= 0)
                return 0;

            var multiplier = Math.Max(0.1d, world.rewardMultiplier);
            var scaled = baseReward * multiplier;
            if (scaled >= long.MaxValue)
                return long.MaxValue;

            return Math.Max(0L, (long)Math.Round(scaled));
        }
    }
}
