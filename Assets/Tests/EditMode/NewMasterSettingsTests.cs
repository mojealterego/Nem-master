using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class NewMasterSettingsTests
    {
        [Test]
        public void DefaultSettingsUseSafeValues()
        {
            var settings = new NewMasterSettings();

            Assert.That(settings.LanguageCode, Is.EqualTo("en"));
            Assert.That(settings.HapticsEnabled, Is.True);
            Assert.That(settings.NotificationsEnabled, Is.True);
            Assert.That(settings.MusicVolume, Is.EqualTo(1f));
            Assert.That(settings.SfxVolume, Is.EqualTo(1f));
        }
    }
}
