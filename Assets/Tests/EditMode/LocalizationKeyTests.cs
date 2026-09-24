using NUnit.Framework;
using NewMaster.Localization;

namespace NewMaster.Tests
{
    public sealed class LocalizationKeyTests
    {
        [Test]
        public void LocalizationKeysRemainStable()
        {
            Assert.That(NewMasterTextKeys.GameReady, Is.EqualTo("game.ready"));
            Assert.That(NewMasterTextKeys.CoinsLabel, Is.EqualTo("hud.coins"));
            Assert.That(NewMasterTextKeys.RaidLoot, Is.EqualTo("raid.loot"));
        }

        [Test]
        public void LocalizationKeyWrapsStableValue()
        {
            var key = new LocalizationKey("test.key");
            Assert.That(key.Value, Is.EqualTo("test.key"));
            Assert.That(key.ToString(), Is.EqualTo("test.key"));
        }
    }
}
