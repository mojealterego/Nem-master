using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Monetization
{
    [CreateAssetMenu(fileName = "BattlePassDefinition", menuName = "New Master/Monetization Battle Pass")]
    public sealed class BattlePassDefinition : ScriptableObject
    {
        [System.Serializable]
        public sealed class Tier
        {
            [Min(1)] public int level = 1;
            [Min(0)] public int experienceRequired;
            [Min(0)] public long coinReward;
            [Min(0)] public int energyReward;
        }

        public string seasonId = "season-1";
        public List<Tier> tiers = new();
    }
}
