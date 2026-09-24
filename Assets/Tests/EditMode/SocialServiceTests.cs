using NUnit.Framework;
using NewMaster.Social;

namespace NewMaster.Tests
{
    public sealed class SocialServiceTests
    {
        [Test]
        public void FriendCannotBeAddedTwice()
        {
            var state = new SocialState { PlayerId = "me" };
            var service = new SocialService();

            Assert.That(service.TryAddFriend(state, "p1"), Is.True);
            Assert.That(service.TryAddFriend(state, "p1"), Is.False);
        }

        [Test]
        public void BlockingRemovesFriend()
        {
            var state = new SocialState { PlayerId = "me" };
            var service = new SocialService();

            service.TryAddFriend(state, "p1");
            Assert.That(service.TryBlock(state, "p1"), Is.True);
            Assert.That(state.FriendIds.Contains("p1"), Is.False);
            Assert.That(state.IsBlocked("p1"), Is.True);
        }
    }
}
