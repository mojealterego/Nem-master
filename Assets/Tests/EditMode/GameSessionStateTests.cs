using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class GameSessionStateTests
    {
        [Test]
        public void ResetClearsCountersAndStoresIdentity()
        {
            var session = new GameSessionState();
            session.SpinsThisSession = 9;
            session.CoinsEarnedThisSession = 42;

            session.Reset(123, 456);

            Assert.That(session.SessionId, Is.EqualTo(123));
            Assert.That(session.StartedAtUnixSeconds, Is.EqualTo(456));
            Assert.That(session.SpinsThisSession, Is.EqualTo(0));
            Assert.That(session.CoinsEarnedThisSession, Is.EqualTo(0));
            Assert.That(session.RaidsThisSession, Is.EqualTo(0));
            Assert.That(session.BuildingsUpgradedThisSession, Is.EqualTo(0));
        }
    }
}
