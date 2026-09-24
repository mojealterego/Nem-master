using System;
using System.Collections.Generic;

namespace NewMaster.Village
{
    [Serializable]
    public sealed class VillageState
    {
        [Serializable]
        public sealed class BuildingProgress
        {
            public int BuildingId;
            public int Level = 1;
            public int Damage;
            public bool HasDefense;
        }

        public List<BuildingProgress> Buildings = new();
        public string CustomizationId = "default";

        public int GetLevel(int buildingId)
        {
            for (var i = 0; i < Buildings.Count; i++)
            {
                var building = Buildings[i];
                if (building != null && building.BuildingId == buildingId)
                    return building.Level;
            }

            return 1;
        }

        public int GetDamage(int buildingId)
        {
            for (var i = 0; i < Buildings.Count; i++)
            {
                var building = Buildings[i];
                if (building != null && building.BuildingId == buildingId)
                    return building.Damage;
            }

            return 0;
        }

        public void Normalize()
        {
            if (Buildings == null)
                Buildings = new List<BuildingProgress>();

            for (var i = Buildings.Count - 1; i >= 0; i--)
            {
                var building = Buildings[i];
                if (building == null || building.BuildingId <= 0)
                {
                    Buildings.RemoveAt(i);
                    continue;
                }

                building.Level = Math.Max(1, building.Level);
                building.Damage = Math.Max(0, building.Damage);
            }

            if (string.IsNullOrWhiteSpace(CustomizationId))
                CustomizationId = "default";
        }
    }
}
