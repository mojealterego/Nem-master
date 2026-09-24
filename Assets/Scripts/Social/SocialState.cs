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

        public void Normalize()
        {
            PlayerId = string.IsNullOrWhiteSpace(PlayerId) ? "local" : PlayerId.Trim();
            FriendIds = NormalizeIds(FriendIds, PlayerId);
            GuildIds = NormalizeIds(GuildIds, null);
            BlockedPlayerIds = NormalizeIds(BlockedPlayerIds, PlayerId);

            for (var i = BlockedPlayerIds.Count - 1; i >= 0; i--)
                FriendIds.Remove(BlockedPlayerIds[i]);
        }

        private static List<string> NormalizeIds(List<string> source, string excludedId)
        {
            var result = new List<string>();
            if (source == null)
                return result;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < source.Count; i++)
            {
                var value = source[i]?.Trim();
                if (string.IsNullOrWhiteSpace(value) ||
                    value == excludedId ||
                    !seen.Add(value))
                    continue;

                result.Add(value);
            }

            return result;
        }
    }
}
