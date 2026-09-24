using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class BattlePassStateTests
    {
        [Test]
        public void NormalizeClampsClaimedTier()
        {
            var state = new BattlePassState
            {
                Experience = -5,
                ClaimedTier = 20
            };

            state.Normalize(10);

            Assert.That(state.Experience, Is.EqualTo(0));
            Assert.That(state.ClaimedTier, Is.EqualTo(10));
        }
    }
}
