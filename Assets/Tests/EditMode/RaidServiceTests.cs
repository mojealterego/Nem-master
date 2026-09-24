using NUnit.Framework;
using NewMaster.Raids;

namespace NewMaster.Tests
{
    public sealed class RaidServiceTests
    {
        [Test]
        public void ShieldBlocksRaidAndConsumesOneShield()
        {
            var target = new RaidTarget
            {
                TargetId = "target-01",
                AvailableLoot = 500,
                ShieldCount = 1
            };

            var result = new RaidService().Resolve(target, true);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Blocked));
            Assert.That(result.ShieldsConsumed, Is.EqualTo(1));
            Assert.That(target.ShieldCount, Is.EqualTo(0));
            Assert.That(target.AvailableLoot, Is.EqualTo(500));
        }

        [Test]
        public void SuccessfulRaidTransfersAvailableLoot()
        {
            var target = new RaidTarget
            {
                TargetId = "target-02",
                AvailableLoot = 750,
                ShieldCount = 0
            };

            var result = new RaidService().Resolve(target, true);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Success));
            Assert.That(result.Loot, Is.EqualTo(750));
            Assert.That(target.AvailableLoot, Is.EqualTo(0));
        }

        [Test]
        public void MissingAttackTokenBlocksRaid()
        {
            var target = new RaidTarget
            {
                AvailableLoot = 1000
            };

            var result = new RaidService().Resolve(target, false);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Blocked));
            Assert.That(target.AvailableLoot, Is.EqualTo(1000));
        }
    }
}
