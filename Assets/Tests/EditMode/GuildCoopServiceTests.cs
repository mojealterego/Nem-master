using NUnit.Framework;
using NewMaster.Social;

namespace NewMaster.Tests
{
    public sealed class GuildCoopServiceTests
    {
        [Test]
        public void ContributionCrossingMilestoneReportsIt()
        {
            var guild = new GuildState();
            var service = new GuildCoopService();

            var result = service.Contribute(guild, 1250, 1000);

            Assert.AreEqual(1250, result.ScoreAdded);
            Assert.AreEqual(1, result.MilestoneReached);
            Assert.AreEqual(1250, guild.CooperativeScore);
        }

        [Test]
        public void MilestoneRewardScalesSafely()
        {
            var reward = new GuildCoopService().GetMilestoneReward(3, 5000);
            Assert.AreEqual(15000, reward);
        }
    }
}
