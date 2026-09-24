using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemMaster.Worlds
{
    [CreateAssetMenu(fileName = "WorldDefinition", menuName = "Nem Master/World Definition")]
    public sealed class WorldDefinition : ScriptableObject
    {
        [Min(1)] public int id = 1;
        public string worldName = "Eldorado";
        public Color themeColor = new(0.1f, 0.08f, 0.16f);
        [Min(0.1f)] public float rewardMultiplier = 1f;
        [Min(0)] public long unlockCost;
        public Sprite background;
        public List<string> symbols = new() { "Coin", "Crown", "Chest", "Energy", "Hammer" };
    }
}
