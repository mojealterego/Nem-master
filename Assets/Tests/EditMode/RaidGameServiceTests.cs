using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Localization;
using NewMaster.Raids;

namespace NewMaster.Tests
{
    public sealed class RaidGameServiceTests
    {
        [Test]
        public void SuccessfulRaidConsumesTokenAndCountsSessionAttempt()
        {
            var game = new GameState { RaidTokens = 1 };
            var target = new RaidTarget
            {
                TargetId = "target-01",
                AvailableLoot = 750,
                ShieldCount = 0
            };

            var result = new RaidGameService().TryRaid(game, target);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Success));
            Assert.That(game.RaidTokens, Is.EqualTo(0));
            Assert.That(game.Coins, Is.EqualTo(750));
            Assert.That(game.Session.RaidsThisSession, Is.EqualTo(1));
            Assert.That(game.Status.Key, Is.EqualTo(NewMasterTextKeys.RaidLoot));
        }

        [Test]
        public void ShieldBlockedRaidConsumesTokenAndCountsSessionAttempt()
        {
            var game = new GameState { RaidTokens = 1 };
            var target = new RaidTarget
            {
                TargetId = "target-02",
                AvailableLoot = 750,
                ShieldCount = 1
            };

            var result = new RaidGameService().TryRaid(game, target);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Blocked));
            Assert.That(result.ShieldsConsumed, Is.EqualTo(1));
            Assert.That(game.RaidTokens, Is.EqualTo(0));
            Assert.That(game.Session.RaidsThisSession, Is.EqualTo(1));
            Assert.That(game.Coins, Is.EqualTo(0));
            Assert.That(game.Status.Key, Is.EqualTo(NewMasterTextKeys.RaidShieldBlocked));
        }

        [Test]
        public void MissingTokenDoesNotCountRaidAttempt()
        {
            var game = new GameState { RaidTokens = 0 };
            var target = new RaidTarget
            {
                TargetId = "target-03",
                AvailableLoot = 750
            };

            var result = new RaidGameService().TryRaid(game, target);

            Assert.That(result.Type, Is.EqualTo(RaidResultType.Blocked));
            Assert.That(game.RaidTokens, Is.EqualTo(0));
            Assert.That(game.Session.RaidsThisSession, Is.EqualTo(0));
            Assert.That(game.Status.Key, Is.EqualTo(NewMasterTextKeys.RaidNoToken));
        }
    }
}
