using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class GameStateMigrationStreakTests
    {
        [Test]
        public void NormalizeClampsAndPreservesBestStreak()
        {
            var state = new GameState
            {
                SpinStreak = -5,
                BestSpinStreak = -10
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.SpinStreak, Is.EqualTo(0));
            Assert.That(state.BestSpinStreak, Is.EqualTo(0));
        }

        [Test]
        public void NormalizeNeverLetsCurrentStreakExceedBest()
        {
            var state = new GameState
            {
                SpinStreak = 12,
                BestSpinStreak = 4
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.BestSpinStreak, Is.EqualTo(12));
        }
    }
}
