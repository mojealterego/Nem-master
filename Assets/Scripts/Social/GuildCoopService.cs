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

    public readonly struct GuildMilestoneClaimResult
    {
        public readonly bool Claimed;
        public readonly int Milestone;
        public readonly long Reward;

        public GuildMilestoneClaimResult(bool claimed, int milestone, long reward)
        {
            Claimed = claimed;
            Milestone = milestone;
            Reward = reward;
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

        public GuildContributionResult Contribute(
            GuildState guild,
            string playerId,
            int amount,
            int milestoneStep = 1000)
        {
            if (guild == null || string.IsNullOrWhiteSpace(playerId) || !guild.HasMember(playerId))
                return new GuildContributionResult(0, 0);

            var result = Contribute(guild, amount, milestoneStep);
            if (result.ScoreAdded > 0)
                guild.AddContribution(playerId.Trim(), result.ScoreAdded);

            return result;
        }

        public bool CanClaimMilestone(
            GuildState guild,
            int milestone,
            int milestoneStep = 1000)
        {
            if (guild == null || milestone <= 0 || milestoneStep <= 0)
                return false;

            var requiredScore = (long)milestone * milestoneStep;
            return requiredScore <= guild.CooperativeScore &&
                   !guild.HasClaimedMilestone(milestone);
        }

        public GuildMilestoneClaimResult PreviewMilestoneClaim(
            GuildState guild,
            int milestone,
            int milestoneStep = 1000,
            long baseReward = 5000)
        {
            if (!CanClaimMilestone(guild, milestone, milestoneStep) || baseReward <= 0)
                return new GuildMilestoneClaimResult(false, milestone, 0);

            return new GuildMilestoneClaimResult(
                true,
                milestone,
                GetMilestoneReward(milestone, baseReward));
        }

        public bool CommitMilestoneClaim(GuildState guild, int milestone)
        {
            return guild != null && guild.MarkMilestoneClaimed(milestone);
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
