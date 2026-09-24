using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class BattlePassProgressionServiceTests
    {
        [Test]
        public void ExperienceProgressionIsClamped()
        {
            var state = new BattlePassState { Experience = int.MaxValue - 5 };

            var added = new BattlePassProgressionService().AddExperience(state, 100);

            Assert.That(added, Is.EqualTo(5));
            Assert.That(state.Experience, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void InvalidProgressDoesNotChangeState()
        {
            var state = new BattlePassState { Experience = 25 };

            var added = new BattlePassProgressionService().AddExperience(state, 0);

            Assert.That(added, Is.EqualTo(0));
            Assert.That(state.Experience, Is.EqualTo(25));
        }
    }
}
