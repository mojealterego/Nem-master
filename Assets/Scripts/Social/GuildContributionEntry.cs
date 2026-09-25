using System;

namespace NewMaster.Social
{
    [Serializable]
    public sealed class GuildContributionEntry
    {
        public string PlayerId;
        public int Score;

        public void Normalize()
        {
            PlayerId = PlayerId?.Trim();
            Score = Math.Max(0, Score);
        }
    }
}
