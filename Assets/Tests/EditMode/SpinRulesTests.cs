using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class SpinRulesTests
    {
        [Test]
        public void TripleMatchProducesJackpot()
        {
            var outcome = SpinRules.Resolve(new[] { "Crown", "Crown", "Crown" }, 100);

            Assert.That(outcome.Type, Is.EqualTo(SpinOutcomeType.Jackpot));
            Assert.That(outcome.Coins, Is.EqualTo(1000));
        }

        [Test]
        public void DoubleCoinProducesCoins()
        {
            var outcome = SpinRules.Resolve(new[] { "Coin", "Coin", "Chest" }, 100);

            Assert.That(outcome.Type, Is.EqualTo(SpinOutcomeType.Coins));
            Assert.That(outcome.Coins, Is.EqualTo(250));
        }

        [Test]
        public void DoubleEnergyProducesEnergy()
        {
            var outcome = SpinRules.Resolve(new[] { "Energy", "Energy", "Chest" }, 100, 3);

            Assert.That(outcome.Type, Is.EqualTo(SpinOutcomeType.Energy));
            Assert.That(outcome.Energy, Is.EqualTo(3));
        }

        [Test]
        public void HammerPairProducesVillageProgress()
        {
            var outcome = SpinRules.Resolve(new[] { "Hammer", "Hammer", "Chest" }, 100);

            Assert.That(outcome.Type, Is.EqualTo(SpinOutcomeType.VillageProgress));
            Assert.That(outcome.VillageProgress, Is.EqualTo(1));
        }

        [Test]
        public void JackpotRewardSaturatesAtLongMaxValue()
        {
            var outcome = SpinRules.Resolve(
                new[] { "Coin", "Coin", "Coin" },
                long.MaxValue);

            Assert.That(outcome.Coins, Is.EqualTo(long.MaxValue));
        }
    }
}
