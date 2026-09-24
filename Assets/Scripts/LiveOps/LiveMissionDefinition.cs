using UnityEngine;

namespace NewMaster.LiveOps
{
    [CreateAssetMenu(fileName = "LiveMissionDefinition", menuName = "New Master/LiveOps Mission")]
    public sealed class LiveMissionDefinition : ScriptableObject
    {
        public string missionId = "mission.example";
        public string localizationKey = "mission.example";
        [Min(1)] public int target = 1;
        [Min(0)] public long coinReward;
        [Min(0)] public int energyReward;
        public string eventId;
    }
}
