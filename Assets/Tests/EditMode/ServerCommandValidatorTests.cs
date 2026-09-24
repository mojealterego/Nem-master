using System;
using NUnit.Framework;
using NewMaster.Online;

namespace NewMaster.Tests
{
    public sealed class ServerCommandValidatorTests
    {
        [Test]
        public void CurrencyAndWorldBoundsAreEnforced()
        {
            var validator = new ServerCommandValidator();
            Assert.That(validator.IsValidCurrencyDelta(100), Is.True);
            Assert.That(validator.IsValidCurrencyDelta(-1), Is.False);
            Assert.That(validator.IsValidWorldId(365), Is.True);
            Assert.That(validator.IsValidWorldId(366), Is.False);
        }

        [Test]
        public void PlayerIdsRejectUnexpectedCharacters()
        {
            var validator = new ServerCommandValidator();

            Assert.That(validator.IsValidPlayerId("player_01"), Is.True);
            Assert.That(validator.IsValidPlayerId("player-01.test"), Is.True);
            Assert.That(validator.IsValidPlayerId("player/01"), Is.False);
            Assert.That(validator.IsValidPlayerId("player 01"), Is.False);
        }

        [Test]
        public void ProductAndModeBoundsAreEnforced()
        {
            var validator = new ServerCommandValidator();

            Assert.That(validator.IsValidProductId("coins.small"), Is.True);
            Assert.That(validator.IsValidProductId(""), Is.False);
            Assert.That(validator.IsValidMode("raid"), Is.True);
            Assert.That(validator.IsValidMode(new string('x', 33)), Is.False);
        }

        [Test]
        public void TimestampMustBeUtc()
        {
            var validator = new ServerCommandValidator();

            Assert.That(validator.IsValidTimestamp(DateTime.UtcNow), Is.True);
            Assert.That(validator.IsValidTimestamp(DateTime.Now), Is.False);
        }
    }
}
