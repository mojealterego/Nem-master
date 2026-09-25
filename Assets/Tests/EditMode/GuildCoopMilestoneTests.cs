using NUnit.Framework;
using NewMaster.Social;

namespace NewMaster.Tests
{
    public sealed class GuildCoopMilestoneTests
    {
        [Test]
        public void ContributionTracksMemberAndCrossedMilestone()
        {
            var guild = new GuildState
            {
                GuildId = "g",
                Name = "Guild",
                LeaderPlayerId = "leader",
                MemberIds = new System.Collections.Generic.List<string> { "leader", "p1" }
            };

            var service = new GuildCoopService();
            var result = service.Contribute(guild, "p1", 1250, 1000);

            Assert.That(result.ScoreAdded, Is.EqualTo(1250));
            Assert.That(result.MilestoneReached, Is.EqualTo(1));
            Assert.That(guild.GetContribution("p1"), Is.EqualTo(1250));
        }

        [Test]
        public void MilestoneClaimIsIdempotent()
        {
            var guild = new GuildState
            {
                GuildId = "g",
                Name = "Guild",
                LeaderPlayerId = "leader",
                MemberIds = new System.Collections.Generic.List<string> { "leader" },
                CooperativeScore = 2000
            };

            var service = new GuildCoopService();
            var preview = service.PreviewMilestoneClaim(guild, 2, 1000, 5000);

            Assert.That(preview.Claimed, Is.True);
            Assert.That(preview.Reward, Is.EqualTo(10000));
            Assert.That(service.CommitMilestoneClaim(guild, 2), Is.True);
            Assert.That(service.CommitMilestoneClaim(guild, 2), Is.False);
            Assert.That(service.CanClaimMilestone(guild, 2, 1000), Is.False);
        }

        [Test]
        public void MilestoneCannotBeClaimedBeforeScore()
        {
            var guild = new GuildState
            {
                GuildId = "g",
                Name = "Guild",
                LeaderPlayerId = "leader",
                MemberIds = new System.Collections.Generic.List<string> { "leader" },
                CooperativeScore = 999
            };

            var service = new GuildCoopService();

            Assert.That(service.CanClaimMilestone(guild, 1, 1000), Is.False);
            Assert.That(service.PreviewMilestoneClaim(guild, 1, 1000, 5000).Claimed, Is.False);
        }
    }
}
