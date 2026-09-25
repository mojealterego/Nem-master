using NUnit.Framework;
using NewMaster.Social;

namespace NewMaster.Tests
{
    public sealed class LeaderboardServiceTests
    {
        [Test]
        public void GuildContributionSnapshotSortsByScoreThenPlayerId()
        {
            var guild = new GuildState
            {
                GuildId = "guild",
                Name = "Guild",
                LeaderPlayerId = "p1",
                MemberIds = new() { "p1", "p2", "p3" }
            };

            guild.AddContribution("p1", 100);
            guild.AddContribution("p2", 300);
            guild.AddContribution("p3", 300);

            var snapshot = new LeaderboardService().BuildGuildContributionSnapshot(guild);

            Assert.That(snapshot.TotalEntries, Is.EqualTo(3));
            Assert.That(snapshot.Entries[0].PlayerId, Is.EqualTo("p2"));
            Assert.That(snapshot.Entries[0].Rank, Is.EqualTo(1));
            Assert.That(snapshot.Entries[1].PlayerId, Is.EqualTo("p3"));
            Assert.That(snapshot.Entries[1].Rank, Is.EqualTo(2));
            Assert.That(snapshot.Entries[2].PlayerId, Is.EqualTo("p1"));
        }

        [Test]
        public void FindRankReturnsZeroWhenPlayerIsOutsideSnapshot()
        {
            var guild = new GuildState
            {
                LeaderPlayerId = "p1",
                MemberIds = new() { "p1", "p2" }
            };

            guild.AddContribution("p1", 10);

            var service = new LeaderboardService();
            var snapshot = service.BuildGuildContributionSnapshot(guild, 1);

            Assert.That(service.FindRank(snapshot, "p2"), Is.EqualTo(0));
        }

        [Test]
        public void SnapshotHandlesNullGuildAndInvalidLimit()
        {
            var service = new LeaderboardService();
            Assert.That(service.BuildGuildContributionSnapshot(null).TotalEntries, Is.EqualTo(0));
            Assert.That(service.BuildGuildContributionSnapshot(new GuildState(), 0).TotalEntries, Is.EqualTo(0));
        }
    }
}
