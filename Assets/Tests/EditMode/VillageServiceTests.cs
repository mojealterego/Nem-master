using NUnit.Framework;
using UnityEngine;
using NewMaster.Core;
using NewMaster.Village;
using NewMaster.Localization;

namespace NewMaster.Tests
{
    public sealed class VillageServiceTests
    {
        [Test]
        public void UpgradeStoresBuildingLocalizationContext()
        {
            var building = ScriptableObject.CreateInstance<BuildingDefinition>();
            building.buildingId = 7;
            building.localizationKey = "building.hammer";
            building.maxLevel = 3;
            building.baseUpgradeCost = 100;
            building.costGrowth = 1.5f;

            try
            {
                var gameState = new GameState { Coins = 500 };
                var villageState = new VillageState();
                var upgraded = new VillageService().TryUpgrade(gameState, villageState, building);

                Assert.That(upgraded, Is.True);
                Assert.That(gameState.Status.Key, Is.EqualTo(NewMasterTextKeys.VillageUpgraded));
                Assert.That(gameState.Status.Amount, Is.EqualTo(1));
                Assert.That(gameState.Status.SecondaryValue, Is.EqualTo(7));
                Assert.That(gameState.Status.Context, Is.EqualTo("building.hammer"));
                Assert.That(gameState.Coins, Is.EqualTo(400));
            }
            finally
            {
                Object.DestroyImmediate(building);
            }
        }

        [Test]
        public void UpgradeStopsAtMaximumLevel()
        {
            var building = ScriptableObject.CreateInstance<BuildingDefinition>();
            building.buildingId = 2;
            building.maxLevel = 1;
            building.baseUpgradeCost = 100;

            try
            {
                var gameState = new GameState { Coins = 500 };
                var villageState = new VillageState();

                Assert.That(new VillageService().TryUpgrade(gameState, villageState, building), Is.False);
                Assert.That(gameState.Coins, Is.EqualTo(500));
            }
            finally
            {
                Object.DestroyImmediate(building);
            }
        }

        [Test]
        public void UpgradeCostSaturatesAtLongMaxValue()
        {
            var building = ScriptableObject.CreateInstance<BuildingDefinition>();
            building.baseUpgradeCost = long.MaxValue;
            building.costGrowth = 2f;

            try
            {
                Assert.That(
                    new VillageService().CalculateUpgradeCost(building, 2),
                    Is.EqualTo(long.MaxValue));
            }
            finally
            {
                Object.DestroyImmediate(building);
            }
        }
    }
}
