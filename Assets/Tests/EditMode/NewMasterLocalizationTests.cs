using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.Tests
{
    public sealed class NewMasterLocalizationTests
    {
        [Test]
        public void DailyRewardStatusFormatsAllArguments()
        {
            var status = new GameStatus();
            status.Set(NewMasterTextKeys.DailyReward, 3, 1500, "1");

            var text = NewMasterLocalization.Get(status);

            Assert.That(text, Does.Contain("3"));
            Assert.That(text, Does.Contain("1,500"));
            Assert.That(text, Does.Contain("1"));
        }

        [Test]
        public void JackpotStatusUsesDedicatedFallback()
        {
            var status = new GameStatus();
            status.Set(NewMasterTextKeys.JackpotReward, 5000);

            var text = NewMasterLocalization.Get(status);

            Assert.That(text, Does.Contain("5,000"));
            Assert.That(text, Does.Contain("JACKPOT"));
        }
    }
}
