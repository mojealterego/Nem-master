using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests.EditMode
{
    public sealed class MasteryPerkServiceTests
    {
        [Test]
        public void CoinBonusUnlocksEveryTwentyFiveRanks()
        {
            var service = new MasteryPerkService();

            Assert.AreEqual(0, service.GetCoinBonusPercent(24));
            Assert.AreEqual(1, service.GetCoinBonusPercent(25));
            Assert.AreEqual(4, service.GetCoinBonusPercent(100));
        }

        [Test]
        public void CoinBonusIsCapped()
        {
            var service = new MasteryPerkService();

            Assert.AreEqual(MasteryPerkService.MaxCoinBonusPercent, service.GetCoinBonusPercent(10000));
        }

        [Test]
        public void CoinBonusPreservesZeroAndSaturates()
        {
            var service = new MasteryPerkService();

            Assert.AreEqual(0, service.ApplyCoinBonus(0, 100));
            Assert.AreEqual(long.MaxValue, service.ApplyCoinBonus(long.MaxValue, 10000));
        }

        [Test]
        public void EnergyBonusUnlocksAtRankOneHundred()
        {
            var service = new MasteryPerkService();

            Assert.AreEqual(0, service.GetEnergyRewardBonus(99));
            Assert.AreEqual(1, service.GetEnergyRewardBonus(100));
        }
    }
}
