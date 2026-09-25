using System;

namespace NewMaster.Raids
{
    [Serializable]
    public sealed class RaidEngineResult
    {
        public RaidResultType Type;
        public long Loot;
        public int ShieldsConsumed;
        public int DefenseMitigationPercent;
        public int VillageDamage;
        public long CounterAttackBounty;

        public bool Succeeded => Type == RaidResultType.Success && Loot > 0;
    }
}
