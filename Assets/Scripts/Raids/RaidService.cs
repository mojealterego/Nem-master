using System;

namespace NewMaster.Raids
{
    public sealed class RaidService
    {
        public RaidResult Resolve(RaidTarget target, bool hasAttackToken)
        {
            if (!hasAttackToken)
                return new RaidResult { Type = RaidResultType.Blocked };

            if (target == null || target.AvailableLoot <= 0)
                return new RaidResult { Type = RaidResultType.Empty };

            if (target.ShieldCount > 0)
            {
                target.ShieldCount--;
                return new RaidResult
                {
                    Type = RaidResultType.Blocked,
                    ShieldsConsumed = 1
                };
            }

            var loot = Math.Max(0L, target.AvailableLoot);
            target.AvailableLoot = 0;

            return new RaidResult
            {
                Type = RaidResultType.Success,
                Loot = loot
            };
        }
    }
}
