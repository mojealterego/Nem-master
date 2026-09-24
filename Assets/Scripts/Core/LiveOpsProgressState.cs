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
            public string EventId;
            public string MissionId;
            public int Progress;
            public bool Claimed;

            public void Normalize()
            {
                EventId ??= string.Empty;
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
                var mission = Missions[i];
                if (mission == null || string.IsNullOrWhiteSpace(mission.MissionId))
                    Missions.RemoveAt(i);
                else
                    mission.Normalize();
            }
        }

        public MissionProgress GetOrCreate(string missionId)
        {
            return GetOrCreate(ActiveEventId, missionId);
        }

        public MissionProgress GetOrCreate(string eventId, string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
                return null;

            Normalize();
            var normalizedEventId = eventId ?? string.Empty;
            for (var i = 0; i < Missions.Count; i++)
            {
                var mission = Missions[i];
                if (mission.MissionId == missionId &&
                    mission.EventId == normalizedEventId)
                    return mission;
            }

            var progress = new MissionProgress
            {
                EventId = normalizedEventId,
                MissionId = missionId
            };
            Missions.Add(progress);
            return progress;
        }

        public void ActivateEvent(string eventId)
        {
            var normalized = eventId ?? string.Empty;
            if (ActiveEventId == normalized)
                return;

            ActiveEventId = normalized;
            Missions.Clear();
        }
    }
}
