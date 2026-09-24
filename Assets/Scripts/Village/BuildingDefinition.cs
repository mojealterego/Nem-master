using UnityEngine;

namespace NewMaster.Village
{
    [CreateAssetMenu(fileName = "BuildingDefinition", menuName = "New Master/Village Building")]
    public sealed class BuildingDefinition : ScriptableObject
    {
        [Min(1)] public int buildingId = 1;
        public string displayName = "Main Building";
        [Min(1)] public int maxLevel = 5;
        [Min(0)] public long baseUpgradeCost = 100;
        [Min(1f)] public float costGrowth = 1.35f;
    }
}
