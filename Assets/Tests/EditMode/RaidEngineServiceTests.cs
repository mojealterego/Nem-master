using NUnit.Framework;
using NewMaster.Raids;

namespace NewMaster.Tests
{
    public sealed class RaidEngineServiceTests
    {
        [Test]
        public void ShieldBlocksAndConsumesOneShield()
        {
            var target = new RaidTarget { AvailableLoot = 1000, ShieldCount = 2 };
            var result = new RaidEngineService().Resolve(target, true);

            Assert.AreEqual(RaidResultType.Blocked, result.Type);
            Assert.AreEqual(1, result.ShieldsConsumed);
            Assert.AreEqual(1, target.ShieldCount);
            Assert.AreEqual(1000, target.AvailableLoot);
        }

        [Test]
        public void DefenseReducesLootButNeverByMoreThanHalf()
        {
            var target = new RaidTarget { AvailableLoot = 1000, DefenseScore = 2000 };
            var result = new RaidEngineService().Resolve(target, true);

            Assert.AreEqual(RaidResultType.Success, result.Type);
            Assert.AreEqual(50, result.DefenseMitigationPercent);
            Assert.AreEqual(500, result.Loot);
        }

        [Test]
        public void SuccessfulRaidClearsTargetLootAndCreatesBounty()
        {
            var target = new RaidTarget { TargetId = "enemy-1", AvailableLoot = 1200, CounterAttackLoot = 200 };
            var result = new RaidEngineService().Resolve(target, true);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, target.AvailableLoot);
            Assert.AreEqual(200, result.CounterAttackBounty);
        }
    }
}
