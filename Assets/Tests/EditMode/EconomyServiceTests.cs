using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class EconomyServiceTests
    {
        [Test]
        public void SpendCoinsRejectsInsufficientFunds()
        {
            var state = new GameState { Coins = 99 };

            Assert.That(new EconomyService().TrySpendCoins(state, 100), Is.False);
            Assert.That(state.Coins, Is.EqualTo(99));
        }

        [Test]
        public void GrantCoinsChangesBalance()
        {
            var state = new GameState();

            Assert.That(new EconomyService().GrantCoins(state, 500), Is.EqualTo(500));
            Assert.That(state.Coins, Is.EqualTo(500));
        }

        [Test]
        public void NegativeTransactionsAreRejected()
        {
            var state = new GameState { Coins = 100, Energy = 5 };
            var economy = new EconomyService();

            Assert.That(economy.TrySpendCoins(state, -1), Is.False);
            Assert.That(economy.GrantCoins(state, -1), Is.EqualTo(0));
            Assert.That(economy.GrantEnergy(state, -1), Is.EqualTo(0));
            Assert.That(state.Coins, Is.EqualTo(100));
            Assert.That(state.Energy, Is.EqualTo(5));
        }

        [Test]
        public void GrantCoinsClampsAtLongMaxValue()
        {
            var state = new GameState { Coins = long.MaxValue - 10 };

            Assert.That(new EconomyService().GrantCoins(state, 100), Is.EqualTo(10));
            Assert.That(state.Coins, Is.EqualTo(long.MaxValue));
        }

        [Test]
        public void GrantEnergyClampsAtIntMaxValue()
        {
            var state = new GameState { Energy = int.MaxValue - 2 };

            Assert.That(new EconomyService().GrantEnergy(state, 100), Is.EqualTo(2));
            Assert.That(state.Energy, Is.EqualTo(int.MaxValue));
        }
    }
}
