using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class GameStateMigrationTests
    {
        [Test]
        public void VersionOneStateReceivesDefaultRaidToken()
        {
            var state = new GameState
            {
                Version = 1,
                RaidTokens = 0
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.Version, Is.EqualTo(GameState.CurrentVersion));
            Assert.That(state.RaidTokens, Is.EqualTo(1));
        }

        [Test]
        public void MigrationClampsInvalidProgressValues()
        {
            var state = new GameState
            {
                Version = 1,
                Energy = -5,
                CurrentWorldId = 0,
                CurrentVillageLevel = 0,
                Slots = null
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.Energy, Is.EqualTo(0));
            Assert.That(state.CurrentWorldId, Is.EqualTo(1));
            Assert.That(state.CurrentVillageLevel, Is.EqualTo(1));
            Assert.That(state.Slots, Is.Not.Null);
        }
    }
}
