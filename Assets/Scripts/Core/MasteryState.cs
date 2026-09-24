using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class MasteryState
    {
        public long Experience;
        public int Rank;
        public int LifetimeMilestones;

        public void Normalize()
        {
            Experience = Math.Max(0L, Experience);
            Rank = Math.Max(0, Rank);
            LifetimeMilestones = Math.Max(0, LifetimeMilestones);
        }
    }
}
