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
        [Test]
        public void MigrationNormalizesBattlePassState()
        {
            var state = new GameState
            {
                BattlePass = new BattlePassState
                {
                    Experience = -50,
                    ClaimedTier = int.MaxValue
                }
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.BattlePass.Experience, Is.EqualTo(0));
            Assert.That(state.BattlePass.ClaimedTier, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void VersionTwoPolishStatusMigratesToStableLocalizationKey()
        {
            var state = new GameState
            {
                Version = 2,
                Status = null,
                StatusMessage = "Postęp wioski +1"
            };

            GameStateMigrations.Normalize(state);

            Assert.That(state.Version, Is.EqualTo(GameState.CurrentVersion));
            Assert.That(state.Status.Key, Is.EqualTo(NewMaster.Localization.NewMasterTextKeys.VillageProgress));
            Assert.That(state.StatusMessage, Is.Null);
        }

    }
}
