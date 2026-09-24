using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.Village
{
    public sealed class VillageService
    {
        private readonly EconomyService economy = new();

        public bool TryUpgrade(
            GameState gameState,
            VillageState villageState,
            BuildingDefinition building)
        {
            if (gameState == null || villageState == null || building == null)
                return false;

            var level = villageState.GetLevel(building.buildingId);
            if (level >= building.maxLevel)
                return false;

            var cost = CalculateUpgradeCost(building, level);
            if (!economy.TrySpendCoins(gameState, cost))
                return false;

            var progress = FindOrCreate(villageState, building.buildingId);
            progress.Level++;
            gameState.CurrentVillageLevel = CalculateVillageLevel(villageState);
            gameState.Status.Set(
                NewMasterTextKeys.VillageUpgraded,
                progress.Level,
                building.buildingId,
                building.localizationKey);
            return true;
        }

        public long CalculateUpgradeCost(BuildingDefinition building, int currentLevel)
        {
            if (building == null)
                return 0;

            var cost = building.baseUpgradeCost;
            for (var level = 1; level < currentLevel; level++)
                cost = (long)(cost * building.costGrowth);

            return cost;
        }

        private static int CalculateVillageLevel(VillageState state)
        {
            var level = 1;
            for (var i = 0; i < state.Buildings.Count; i++)
            {
                if (state.Buildings[i] != null)
                    level = System.Math.Max(level, state.Buildings[i].Level);
            }

            return level;
        }

        private static VillageState.BuildingProgress FindOrCreate(VillageState state, int buildingId)
        {
            for (var i = 0; i < state.Buildings.Count; i++)
            {
                if (state.Buildings[i].BuildingId == buildingId)
                    return state.Buildings[i];
            }

            var created = new VillageState.BuildingProgress { BuildingId = buildingId };
            state.Buildings.Add(created);
            return created;
        }
    }
}
