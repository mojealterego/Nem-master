using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class NewMasterHapticsServiceTests
    {
        [Test]
        public void HapticsServiceCanBeConstructed()
        {
            Assert.That(new NewMasterHapticsService(), Is.Not.Null);
        }
    }
}
