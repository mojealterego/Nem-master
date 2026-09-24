using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests.EditMode
{
    public sealed class LiveOpsProgressServiceTests
    {
        [Test]
        public void ProgressIsCreatedAndCapped()
        {
            var service = new LiveOpsProgressService();
            var state = new LiveOpsProgressState();

            Assert.That(service.AddProgress(state, "spin.50", 60, 50), Is.True);
            Assert.That(state.Missions[0].Progress, Is.EqualTo(50));
        }

        [Test]
        public void ClaimedMissionCannotAdvance()
        {
            var service = new LiveOpsProgressService();
            var state = new LiveOpsProgressState();
            service.AddProgress(state, "spin.50", 50, 50);
            service.TryClaim(state, "spin.50", 50, 1000, 1, new GameState());

            Assert.That(service.AddProgress(state, "spin.50", 1, 50), Is.False);
        }

        [Test]
        public void ClaimRequiresTarget()
        {
            var service = new LiveOpsProgressService();
            var state = new LiveOpsProgressState();
            var game = new GameState();

            Assert.That(service.TryClaim(state, "spin.50", 50, 1000, 1, game), Is.False);
            Assert.That(game.Coins, Is.EqualTo(0));
        }

        [Test]
        public void ClaimGrantsConfiguredRewardsOnce()
        {
            var service = new LiveOpsProgressService();
            var state = new LiveOpsProgressState();
            var game = new GameState();
            service.AddProgress(state, "spin.50", 50, 50);

            Assert.That(service.TryClaim(state, "spin.50", 50, 1000, 2, game), Is.True);
            Assert.That(game.Coins, Is.EqualTo(1000));
            Assert.That(game.Energy, Is.EqualTo(12));
            Assert.That(service.TryClaim(state, "spin.50", 50, 1000, 2, game), Is.False);
        }
    }
}
