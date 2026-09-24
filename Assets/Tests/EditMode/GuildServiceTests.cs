using NUnit.Framework;
using NewMaster.Social;

namespace NewMaster.Tests
{
    public sealed class GuildServiceTests
    {
        [Test]
        public void CreateGuildAddsLeader()
        {
            var guild = new GuildState();
            Assert.That(new GuildService().TryCreate(guild, "guild-1", "New Master", "player-1"), Is.True);
            Assert.That(guild.HasMember("player-1"), Is.True);
        }

        [Test]
        public void LeaderCannotBeRemoved()
        {
            var guild = new GuildState
            {
                LeaderPlayerId = "leader",
                MemberIds = new System.Collections.Generic.List<string> { "leader", "member" }
            };

            Assert.That(new GuildService().TryRemoveMember(guild, "leader"), Is.False);
            Assert.That(guild.HasMember("leader"), Is.True);
        }

        [Test]
        public void GuildNormalizesDuplicateMembers()
        {
            var guild = new GuildState
            {
                LeaderPlayerId = "leader",
                MemberIds = new System.Collections.Generic.List<string> { "leader", "leader", " member " }
            };

            guild.Normalize();

            Assert.That(guild.MemberIds, Is.EqualTo(new[] { "leader", "member" }));
        }
    }
}
