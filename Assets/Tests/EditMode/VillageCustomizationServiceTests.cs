using NUnit.Framework;
using NewMaster.Village;

namespace NewMaster.Tests
{
    public sealed class VillageCustomizationServiceTests
    {
        [Test]
        public void CustomizationIdIsPersisted()
        {
            var state = new VillageState();
            Assert.That(new VillageCustomizationService().TrySetCustomization(state, "winter"), Is.True);
            Assert.That(state.CustomizationId, Is.EqualTo("winter"));
        }
    }
}
