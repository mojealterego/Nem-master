using NUnit.Framework;

namespace NewMaster.Tests
{
    public sealed class NewMasterInputBootstrapTests
    {
        [Test]
        public void InputBootstrapTypeExists()
        {
            Assert.That(typeof(NewMaster.Bootstrap.NewMasterInputBootstrap).Name, Is.EqualTo("NewMasterInputBootstrap"));
        }
    }
}
