using System;

namespace NewMaster.Raids
{
    public sealed class RaidEngineService
    {
        public RaidEngineResult Resolve(RaidTarget target, bool hasAttackToken)
        {
            if (!hasAttackToken)
                return new RaidEngineResult { Type = RaidResultType.Blocked };

            if (target == null || target.AvailableLoot <= 0)
                return new RaidEngineResult { Type = RaidResultType.Empty };

            if (target.ShieldCount > 0)
            {
                target.ShieldCount--;
                return new RaidEngineResult
                {
                    Type = RaidResultType.Blocked,
                    ShieldsConsumed = 1
                };
            }

            var mitigation = Math.Min(50, Math.Max(0, target.DefenseScore) / 40);
            var loot = target.AvailableLoot;
            var protectedLoot = (long)Math.Floor(loot * (mitigation / 100.0));
            var awardedLoot = Math.Max(0L, loot - protectedLoot);
            var villageDamage = Math.Max(0, Math.Max(0, target.CounterAttackPower) - Math.Max(0, target.DefenseScore / 10));
            var bountyBase = target.CounterAttackLoot > 0 ? target.CounterAttackLoot : awardedLoot / 4;
            var bounty = Math.Min(awardedLoot / 2, Math.Max(0L, bountyBase));

            target.AvailableLoot = 0;
            return new RaidEngineResult
            {
                Type = RaidResultType.Success,
                Loot = awardedLoot,
                DefenseMitigationPercent = mitigation,
                VillageDamage = villageDamage,
                CounterAttackBounty = bounty
            };
        }
    }
}
