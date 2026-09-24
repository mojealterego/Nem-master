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
        }

        public List<BuildingProgress> Buildings = new();

        public int GetLevel(int buildingId)
        {
            for (var i = 0; i < Buildings.Count; i++)
            {
                if (Buildings[i].BuildingId == buildingId)
                    return Buildings[i].Level;
            }

            return 1;
        }
    }
}
