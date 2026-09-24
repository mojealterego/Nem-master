using NUnit.Framework;
using NewMaster.Localization;

namespace NewMaster.Tests
{
    public sealed class LocalizationFallbackTests
    {
        [Test]
        public void UnknownWorldKeyUsesDeterministicFallback()
        {
            Assert.That(
                NewMasterLocalization.Get("world.365"),
                Is.EqualTo("World 365"));
        }
    }
}
