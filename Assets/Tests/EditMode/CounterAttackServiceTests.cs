using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class CounterAttackServiceTests
    {
        [Test]
        public void ClaimPaysAndClearsBounty()
        {
            var state = new CounterAttackState();
            state.Set("enemy-1", 500);
            var game = new GameState();

            Assert.IsTrue(new CounterAttackService().TryClaim(state, game, new EconomyService()));
            Assert.AreEqual(500, game.Coins);
            Assert.IsFalse(state.Available);
            Assert.AreEqual(0, state.Bounty);
        }

        [Test]
        public void ClaimDoesNotClearBountyWhenCurrencyIsAtCap()
        {
            var state = new CounterAttackState();
            state.Set("enemy-1", 500);
            var game = new GameState { Coins = long.MaxValue };

            Assert.IsFalse(new CounterAttackService().TryClaim(state, game, new EconomyService()));
            Assert.IsTrue(state.Available);
            Assert.AreEqual(500, state.Bounty);
        }
    }
}
