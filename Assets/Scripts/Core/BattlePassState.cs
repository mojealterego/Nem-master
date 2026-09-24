using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class BattlePassState
    {
        public string SeasonId = "season-1";
        public int Experience;
        public int ClaimedTier;

        public void Normalize(int maxTier)
        {
            Experience = Math.Max(0, Experience);
            ClaimedTier = Math.Max(0, ClaimedTier);
            if (maxTier > 0)
                ClaimedTier = Math.Min(ClaimedTier, maxTier);
        }
    }
}
