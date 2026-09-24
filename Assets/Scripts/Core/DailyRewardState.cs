using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class DailyRewardState
    {
        public string LastClaimDateUtc;
        public int StreakDay;
        public int TotalClaims;

        public void Normalize()
        {
            StreakDay = Math.Max(0, Math.Min(7, StreakDay));
            TotalClaims = Math.Max(0, TotalClaims);

            if (string.IsNullOrWhiteSpace(LastClaimDateUtc))
                StreakDay = 0;
        }

        public void Reset()
        {
            LastClaimDateUtc = null;
            StreakDay = 0;
            TotalClaims = 0;
        }
    }
}
