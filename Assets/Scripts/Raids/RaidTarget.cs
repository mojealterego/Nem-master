using System;

namespace NewMaster.Raids
{
    [Serializable]
    public sealed class RaidTarget
    {
        public string TargetId;
        public string DisplayName;
        public long AvailableLoot;
        public int ShieldCount;
    }
}
