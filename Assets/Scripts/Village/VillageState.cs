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
                if (Buildings[i].BuildingId == buildingId) return Buildings[i].Level;
            return 1;
        }

        public int GetDamage(int buildingId)
        {
            for (var i = 0; i < Buildings.Count; i++)
                if (Buildings[i].BuildingId == buildingId) return Buildings[i].Damage;
            return 0;
        }
    }
}
