using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Village;

namespace NewMaster.Tests
{
    public sealed class VillageDefenseServiceTests
    {
        [Test]
        public void DamageCanBeAppliedAndRepaired()
        {
            var state = new GameState { Coins = 1000 };
            var village = new VillageState();
            var building = new BuildingDefinition { buildingId = 1 };
            var service = new VillageDefenseService();

            Assert.That(service.ApplyDamage(village, 1, 3), Is.True);
            Assert.That(village.GetDamage(1), Is.EqualTo(3));
            Assert.That(service.TryRepair(state, village, building, 100), Is.True);
            Assert.That(village.GetDamage(1), Is.EqualTo(0));
            Assert.That(state.Coins, Is.EqualTo(900));
        }
    }
}
