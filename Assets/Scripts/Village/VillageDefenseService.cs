using NewMaster.Core;
namespace NewMaster.Village
{
    public sealed class VillageDefenseService
    {
        public bool TryRepair(GameState state, VillageState village, BuildingDefinition building, long repairCost)
        {
            if (state == null || village == null || building == null || repairCost < 0) return false;
            var progress = Find(village, building.buildingId);
            if (progress == null || progress.Damage <= 0) return false;
            if (!new EconomyService().TrySpendCoins(state, repairCost)) return false;
            progress.Damage = 0;
            return true;
        }

        public bool ApplyDamage(VillageState village, int buildingId, int amount)
        {
            if (village == null || amount <= 0) return false;
            var progress = Find(village, buildingId);
            if (progress == null)
            {
                progress = new VillageState.BuildingProgress { BuildingId = buildingId };
                village.Buildings.Add(progress);
            }
            progress.Damage += amount;
            return true;
        }

        public bool SetDefense(VillageState village, int buildingId, bool enabled)
        {
            var progress = Find(village, buildingId);
            if (progress == null)
            {
                if (!enabled) return false;
                progress = new VillageState.BuildingProgress { BuildingId = buildingId };
                village.Buildings.Add(progress);
            }
            progress.HasDefense = enabled;
            return true;
        }

        private static VillageState.BuildingProgress Find(VillageState village, int buildingId)
        {
            for (var i = 0; i < village.Buildings.Count; i++)
                if (village.Buildings[i].BuildingId == buildingId) return village.Buildings[i];
            return null;
        }
    }
}
