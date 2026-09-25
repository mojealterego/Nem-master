using System;
using System.Collections.Generic;

namespace NewMaster.Social
{
    public readonly struct LeaderboardEntry
    {
        public readonly string PlayerId;
        public readonly string DisplayName;
        public readonly long Score;
        public readonly int Rank;

        public LeaderboardEntry(string playerId, string displayName, long score, int rank = 0)
        {
            PlayerId = playerId;
            DisplayName = displayName;
            Score = Math.Max(0L, score);
            Rank = Math.Max(0, rank);
        }
    }

    public sealed class LeaderboardSnapshot
    {
        private readonly List<LeaderboardEntry> entries = new();
        public IReadOnlyList<LeaderboardEntry> Entries => entries;
        public int TotalEntries => entries.Count;
        internal void Add(LeaderboardEntry entry) => entries.Add(entry);
    }

    public sealed class LeaderboardService
    {
        public LeaderboardSnapshot BuildGuildContributionSnapshot(GuildState guild, int maxEntries = 20)
        {
            var snapshot = new LeaderboardSnapshot();
            if (guild == null || maxEntries <= 0)
                return snapshot;

            guild.Normalize();
            var candidates = new List<LeaderboardEntry>();
            for (var i = 0; i < guild.MemberIds.Count; i++)
            {
                var playerId = guild.MemberIds[i];
                candidates.Add(new LeaderboardEntry(playerId, playerId, guild.GetContribution(playerId)));
            }

            candidates.Sort(Compare);
            var limit = Math.Min(maxEntries, candidates.Count);
            for (var i = 0; i < limit; i++)
            {
                var item = candidates[i];
                snapshot.Add(new LeaderboardEntry(item.PlayerId, item.DisplayName, item.Score, i + 1));
            }

            return snapshot;
        }

        public int FindRank(LeaderboardSnapshot snapshot, string playerId)
        {
            if (snapshot == null || string.IsNullOrWhiteSpace(playerId))
                return 0;

            for (var i = 0; i < snapshot.Entries.Count; i++)
            {
                if (string.Equals(snapshot.Entries[i].PlayerId, playerId.Trim(), StringComparison.Ordinal))
                    return snapshot.Entries[i].Rank;
            }

            return 0;
        }

        private static int Compare(LeaderboardEntry left, LeaderboardEntry right)
        {
            var score = right.Score.CompareTo(left.Score);
            return score != 0 ? score : string.Compare(left.PlayerId, right.PlayerId, StringComparison.Ordinal);
        }
    }
}
