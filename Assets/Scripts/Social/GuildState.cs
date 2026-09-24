using System;
using System.Collections.Generic;

namespace NewMaster.Social
{
    [Serializable]
    public sealed class GuildState
    {
        public string GuildId;
        public string Name;
        public string LeaderPlayerId;
        public List<string> MemberIds = new();
        public int CooperativeScore;

        public bool HasMember(string playerId) =>
            !string.IsNullOrWhiteSpace(playerId) && MemberIds.Contains(playerId);

        public void Normalize()
        {
            GuildId = GuildId?.Trim();
            Name = Name?.Trim();
            LeaderPlayerId = LeaderPlayerId?.Trim();
            MemberIds = NormalizeMembers(MemberIds);
            CooperativeScore = Math.Max(0, CooperativeScore);

            if (!string.IsNullOrWhiteSpace(LeaderPlayerId) && !HasMember(LeaderPlayerId))
                MemberIds.Insert(0, LeaderPlayerId);
        }

        private static List<string> NormalizeMembers(List<string> source)
        {
            var result = new List<string>();
            if (source == null)
                return result;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < source.Count; i++)
            {
                var id = source[i]?.Trim();
                if (string.IsNullOrWhiteSpace(id) || !seen.Add(id))
                    continue;

                result.Add(id);
            }

            return result;
        }
    }
}
