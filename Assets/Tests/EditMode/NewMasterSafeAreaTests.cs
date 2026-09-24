using NUnit.Framework;

namespace NewMaster.Tests
{
    public sealed class NewMasterSafeAreaTests
    {
        [Test]
        public void SafeAreaComponentHasDedicatedRuntimeType()
        {
            Assert.That(typeof(NewMaster.UI.NewMasterSafeArea).Name, Is.EqualTo("NewMasterSafeArea"));
        }
    }
}
