using System;

namespace NewMaster.Raids
{
    public enum RaidResultType
    {
        Blocked,
        Success,
        Empty
    }

    [Serializable]
    public sealed class RaidResult
    {
        public RaidResultType Type;
        public long Loot;
        public int ShieldsConsumed;
    }
}
