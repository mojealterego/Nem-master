using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Worlds
{
    [CreateAssetMenu(fileName = "WorldDefinition", menuName = "New Master/World Definition")]
    public sealed class WorldDefinition : ScriptableObject
    {
        [Min(1)] public int id = 1;
        public string worldName = "World 001";
        public string localizationKey = "world.001";
        public Color themeColor = new(0.1f, 0.08f, 0.16f);
        [Min(0.1f)] public float rewardMultiplier = 1f;
        [Min(0)] public long unlockCost;
        [Min(1)] public long baseSpinReward = 100;
        [Min(0)] public int energyReward = 1;
        public Sprite background;
        public List<string> symbols = new() { "Coin", "Crown", "Chest", "Energy", "Hammer" };
    }
}
