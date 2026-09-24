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
            SessionId = sessionId;
            StartedAtUnixSeconds = startedAtUnixSeconds;
            SpinsThisSession = 0;
            CoinsEarnedThisSession = 0;
            RaidsThisSession = 0;
            BuildingsUpgradedThisSession = 0;
        }
    }
}
