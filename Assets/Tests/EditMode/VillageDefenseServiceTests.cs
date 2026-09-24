using NUnit.Framework;
using UnityEngine;
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
            var building = ScriptableObject.CreateInstance<BuildingDefinition>();
            building.buildingId = 1;

            try
            {
                var service = new VillageDefenseService();

                Assert.That(service.ApplyDamage(village, 1, 3), Is.True);
                Assert.That(village.GetDamage(1), Is.EqualTo(3));
                Assert.That(service.TryRepair(state, village, building, 100), Is.True);
                Assert.That(village.GetDamage(1), Is.EqualTo(0));
                Assert.That(state.Coins, Is.EqualTo(900));
            }
            finally
            {
                Object.DestroyImmediate(building);
            }
        }

        [Test]
        public void DamageSaturatesInsteadOfOverflowing()
        {
            var village = new VillageState();
            var service = new VillageDefenseService();

            Assert.That(service.ApplyDamage(village, 1, int.MaxValue), Is.True);
            Assert.That(service.ApplyDamage(village, 1, 1), Is.False);
            Assert.That(village.GetDamage(1), Is.EqualTo(int.MaxValue));
        }
    }
}
