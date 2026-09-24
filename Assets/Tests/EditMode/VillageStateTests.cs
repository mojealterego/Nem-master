using NUnit.Framework;
using NewMaster.Village;

namespace NewMaster.Tests
{
    public sealed class VillageStateTests
    {
        [Test]
        public void NormalizeRemovesInvalidBuildingsAndClampsValues()
        {
            var state = new VillageState();
            state.Buildings.Add(null);
            state.Buildings.Add(new VillageState.BuildingProgress
            {
                BuildingId = 0,
                Level = -4,
                Damage = -2
            });
            state.Buildings.Add(new VillageState.BuildingProgress
            {
                BuildingId = 7,
                Level = 0,
                Damage = -3
            });
            state.CustomizationId = string.Empty;

            state.Normalize();

            Assert.That(state.Buildings.Count, Is.EqualTo(1));
            Assert.That(state.Buildings[0].BuildingId, Is.EqualTo(7));
            Assert.That(state.Buildings[0].Level, Is.EqualTo(1));
            Assert.That(state.Buildings[0].Damage, Is.EqualTo(0));
            Assert.That(state.CustomizationId, Is.EqualTo("default"));
        }
    }
}
