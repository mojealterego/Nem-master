using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class GameSessionState
    {
        public long SessionId;
        public long StartedAtUnixSeconds;
        public int SpinsThisSession;
        public int CoinsEarnedThisSession;
        public int RaidsThisSession;
        public int BuildingsUpgradedThisSession;

        public void Reset(long sessionId, long startedAtUnixSeconds)
        {
            SessionId = Math.Max(0L, sessionId);
            StartedAtUnixSeconds = Math.Max(0L, startedAtUnixSeconds);
            SpinsThisSession = 0;
            CoinsEarnedThisSession = 0;
            RaidsThisSession = 0;
            BuildingsUpgradedThisSession = 0;
        }

        public void Normalize()
        {
            SessionId = Math.Max(0L, SessionId);
            StartedAtUnixSeconds = Math.Max(0L, StartedAtUnixSeconds);
            SpinsThisSession = Math.Max(0, SpinsThisSession);
            CoinsEarnedThisSession = Math.Max(0, CoinsEarnedThisSession);
            RaidsThisSession = Math.Max(0, RaidsThisSession);
            BuildingsUpgradedThisSession = Math.Max(0, BuildingsUpgradedThisSession);

            if (SessionId == 0)
                StartedAtUnixSeconds = 0;
        }
    }
}
