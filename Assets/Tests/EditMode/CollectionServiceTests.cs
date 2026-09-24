using NUnit.Framework;
using NewMaster.Collections;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class CollectionServiceTests
    {
        [Test]
        public void CompletedSetGrantsReward()
        {
            var set = new CollectionSet
            {
                Id = "set-01",
                DisplayName = "Pierwszy Album",
                CompletionReward = 1000,
                RequiredCards = { "card-a", "card-b" }
            };
            var state = new CollectionState();
            state.OwnedCardIds.Add("card-a");
            state.OwnedCardIds.Add("card-b");
            var game = new GameState();

            Assert.That(new CollectionService().TryComplete(set, state, game), Is.True);
            Assert.That(game.Coins, Is.EqualTo(1000));
        }

        [Test]
        public void CompletedSetCannotBeClaimedTwice()
        {
            var set = new CollectionSet
            {
                Id = "set-03",
                DisplayName = "Trzeci Album",
                CompletionReward = 1000,
                RequiredCards = { "card-a" }
            };
            var state = new CollectionState();
            state.OwnedCardIds.Add("card-a");
            var game = new GameState();
            var service = new CollectionService();

            Assert.That(service.TryComplete(set, state, game), Is.True);
            Assert.That(service.TryComplete(set, state, game), Is.False);
            Assert.That(game.Coins, Is.EqualTo(1000));
        }

        [Test]
        public void IncompleteSetDoesNotGrantReward()
        {
            var set = new CollectionSet
            {
                Id = "set-02",
                DisplayName = "Drugi Album",
                CompletionReward = 1000,
                RequiredCards = { "card-a", "card-b" }
            };
            var state = new CollectionState();
            state.OwnedCardIds.Add("card-a");
            var game = new GameState();

            Assert.That(new CollectionService().TryComplete(set, state, game), Is.False);
            Assert.That(game.Coins, Is.EqualTo(0));
        }
    }
}
