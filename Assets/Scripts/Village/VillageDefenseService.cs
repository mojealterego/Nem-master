using NewMaster.Core;

namespace NewMaster.Village
{
    public sealed class VillageDefenseService
    {
        public bool TryRepair(GameState state, VillageState village, BuildingDefinition building, long repairCost)
        {
            if (state == null || village == null || building == null || repairCost < 0)
                return false;

            var progress = Find(village, building.buildingId);
            if (progress == null || progress.Damage <= 0)
                return false;

            if (!new EconomyService().TrySpendCoins(state, repairCost))
                return false;

            progress.Damage = 0;
            return true;
        }

        public bool ApplyDamage(VillageState village, int buildingId, int amount)
        {
            if (village == null || buildingId <= 0 || amount <= 0)
                return false;

            var progress = Find(village, buildingId);
            if (progress == null)
            {
                progress = new VillageState.BuildingProgress { BuildingId = buildingId };
                village.Buildings.Add(progress);
            }

            var safeDamage = System.Math.Min(
                amount,
                int.MaxValue - progress.Damage);
            progress.Damage += safeDamage;
            return safeDamage > 0;
        }

        public bool SetDefense(VillageState village, int buildingId, bool enabled)
        {
            if (village == null || buildingId <= 0)
                return false;

            var progress = Find(village, buildingId);
            if (progress == null)
            {
                if (!enabled)
                    return false;

                progress = new VillageState.BuildingProgress { BuildingId = buildingId };
                village.Buildings.Add(progress);
            }

            progress.HasDefense = enabled;
            return true;
        }

        private static VillageState.BuildingProgress Find(VillageState village, int buildingId)
        {
            for (var i = 0; i < village.Buildings.Count; i++)
            {
                if (village.Buildings[i].BuildingId == buildingId)
                    return village.Buildings[i];
            }

            return null;
        }
    }
}
