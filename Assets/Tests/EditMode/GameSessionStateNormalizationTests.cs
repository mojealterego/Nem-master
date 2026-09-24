using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class GameSessionStateNormalizationTests
    {
        [Test]
        public void NormalizeClampsNegativeTelemetry()
        {
            var session = new GameSessionState
            {
                SessionId = -10,
                StartedAtUnixSeconds = -20,
                SpinsThisSession = -1,
                CoinsEarnedThisSession = -2,
                RaidsThisSession = -3,
                BuildingsUpgradedThisSession = -4
            };

            session.Normalize();

            Assert.That(session.SessionId, Is.EqualTo(0));
            Assert.That(session.StartedAtUnixSeconds, Is.EqualTo(0));
            Assert.That(session.SpinsThisSession, Is.EqualTo(0));
            Assert.That(session.CoinsEarnedThisSession, Is.EqualTo(0));
            Assert.That(session.RaidsThisSession, Is.EqualTo(0));
            Assert.That(session.BuildingsUpgradedThisSession, Is.EqualTo(0));
        }

        [Test]
        public void ResetClampsInvalidIdentifiers()
        {
            var session = new GameSessionState();

            session.Reset(-1, -2);

            Assert.That(session.SessionId, Is.EqualTo(0));
            Assert.That(session.StartedAtUnixSeconds, Is.EqualTo(0));
            Assert.That(session.SpinsThisSession, Is.EqualTo(0));
        }
    }
}
