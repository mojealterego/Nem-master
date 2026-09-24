using NewMaster.Core;

namespace NewMaster.Village
{
    public sealed class VillageService
    {
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
            if (gameState.Coins < cost)
                return false;

            gameState.Coins -= cost;

            var progress = FindOrCreate(villageState, building.buildingId);
            progress.Level++;
            gameState.CurrentVillageLevel = progress.Level;
            gameState.StatusMessage = $"{building.displayName}: poziom {progress.Level}";
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
