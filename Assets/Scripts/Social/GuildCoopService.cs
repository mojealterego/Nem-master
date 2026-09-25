using System;

namespace NewMaster.Social
{
    public readonly struct GuildContributionResult
    {
        public readonly int ScoreAdded;
        public readonly int MilestoneReached;

        public GuildContributionResult(int scoreAdded, int milestoneReached)
        {
            ScoreAdded = scoreAdded;
            MilestoneReached = milestoneReached;
        }
    }

    public sealed class GuildCoopService
    {
        public GuildContributionResult Contribute(GuildState guild, int amount, int milestoneStep = 1000)
        {
            if (guild == null || amount <= 0 || milestoneStep <= 0)
                return new GuildContributionResult(0, 0);

            var before = guild.CooperativeScore;
            var safe = Math.Min(amount, int.MaxValue - before);
            if (safe <= 0)
                return new GuildContributionResult(0, 0);

            guild.CooperativeScore += safe;
            var previousMilestone = before / milestoneStep;
            var currentMilestone = guild.CooperativeScore / milestoneStep;
            return new GuildContributionResult(safe, Math.Max(0, currentMilestone - previousMilestone));
        }

        public long GetMilestoneReward(int milestone, long baseReward = 5000)
        {
            if (milestone <= 0 || baseReward <= 0)
                return 0;

            if (milestone > long.MaxValue / baseReward)
                return long.MaxValue;

            return baseReward * milestone;
        }
    }
}
