using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class GameStatusTests
    {
        [Test]
        public void GameStatusStoresStableKeyAndArguments()
        {
            var status = new GameStatus();
            status.Set("game.coins_reward", 1250);

            Assert.That(status.Key, Is.EqualTo("game.coins_reward"));
            Assert.That(status.Amount, Is.EqualTo(1250));
        }

        [Test]
        public void GameStatusCanStoreSecondaryValueAndContext()
        {
            var status = new GameStatus();
            status.Set("village.upgraded", 4, 2, "building.hammer");

            Assert.That(status.Amount, Is.EqualTo(4));
            Assert.That(status.SecondaryValue, Is.EqualTo(2));
            Assert.That(status.Context, Is.EqualTo("building.hammer"));
        }
    }
}
