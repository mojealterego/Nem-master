using System;
using System.Collections.Generic;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class LiveOpsProgressState
    {
        [Serializable]
        public sealed class MissionProgress
        {
            public string MissionId;
            public int Progress;
            public bool Claimed;

            public void Normalize()
            {
                MissionId ??= string.Empty;
                Progress = Math.Max(0, Progress);
            }
        }

        public string ActiveEventId;
        public List<MissionProgress> Missions = new();

        public void Normalize()
        {
            Missions ??= new List<MissionProgress>();
            for (var i = Missions.Count - 1; i >= 0; i--)
            {
                if (Missions[i] == null || string.IsNullOrWhiteSpace(Missions[i].MissionId))
                    Missions.RemoveAt(i);
                else
                    Missions[i].Normalize();
            }
        }

        public MissionProgress GetOrCreate(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
                return null;

            Normalize();
            for (var i = 0; i < Missions.Count; i++)
            {
                if (Missions[i].MissionId == missionId)
                    return Missions[i];
            }

            var progress = new MissionProgress { MissionId = missionId };
            Missions.Add(progress);
            return progress;
        }
    }
}
