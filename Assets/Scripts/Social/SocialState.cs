using System;
using System.Collections.Generic;

namespace NewMaster.Social
{
    [Serializable]
    public sealed class SocialState
    {
        public string PlayerId = "local";
        public List<string> FriendIds = new();
        public List<string> GuildIds = new();
        public List<string> BlockedPlayerIds = new();

        public bool IsBlocked(string playerId) =>
            !string.IsNullOrWhiteSpace(playerId) && BlockedPlayerIds.Contains(playerId);
    }
}
