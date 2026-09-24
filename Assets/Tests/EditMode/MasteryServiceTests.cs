using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests.EditMode
{
    public sealed class MasteryServiceTests
    {
        [Test]
        public void AddExperienceUpdatesRank()
        {
            var service = new MasteryService();
            var state = new MasteryState();

            service.AddExperience(state, 500);

            Assert.That(state.Experience, Is.EqualTo(500));
            Assert.That(state.Rank, Is.EqualTo(1));
        }

        [Test]
        public void AddExperienceIgnoresInvalidAmounts()
        {
            var service = new MasteryService();
            var state = new MasteryState { Experience = 100, Rank = 0 };

            var added = service.AddExperience(state, -5);

            Assert.That(added, Is.EqualTo(0));
            Assert.That(state.Experience, Is.EqualTo(100));
        }

        [Test]
        public void AddExperienceSaturatesAtLongMaxValue()
        {
            var service = new MasteryService();
            var state = new MasteryState { Experience = long.MaxValue - 10 };

            service.AddExperience(state, 100);

            Assert.That(state.Experience, Is.EqualTo(long.MaxValue));
        }

        [Test]
        public void MilestonesReflectRankTransitions()
        {
            var service = new MasteryService();

            var one = service.GetExperienceForNextRank(0);
            var two = service.GetExperienceForNextRank(1);

            Assert.That(service.GetMilestonesReached(0, one), Is.EqualTo(1));
            Assert.That(service.GetMilestonesReached(one, one + two), Is.EqualTo(1));
        }
    }
}
