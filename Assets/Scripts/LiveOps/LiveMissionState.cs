using System;

namespace NewMaster.LiveOps
{
    [Serializable]
    public sealed class LiveMissionState
    {
        public string MissionId;
        public int Progress;
        public bool Claimed;

        public bool IsComplete(int target) => target > 0 && Progress >= target;

        public void Normalize(int target)
        {
            Progress = Math.Max(0, Progress);
            if (target > 0)
                Progress = Math.Min(Progress, target);
        }
    }
}
