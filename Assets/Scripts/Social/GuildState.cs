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
        public List<GuildContributionEntry> Contributions = new();
        public List<int> ClaimedMilestones = new();

        public bool HasMember(string playerId) =>
            !string.IsNullOrWhiteSpace(playerId) && MemberIds.Contains(playerId);

        public int GetContribution(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId) || Contributions == null)
                return 0;

            for (var i = 0; i < Contributions.Count; i++)
            {
                var entry = Contributions[i];
                if (entry != null && entry.PlayerId == playerId)
                    return Math.Max(0, entry.Score);
            }

            return 0;
        }

        public bool HasClaimedMilestone(int milestone) =>
            milestone > 0 && ClaimedMilestones != null && ClaimedMilestones.Contains(milestone);

        public void Normalize()
        {
            GuildId = GuildId?.Trim();
            Name = Name?.Trim();
            LeaderPlayerId = LeaderPlayerId?.Trim();
            MemberIds = NormalizeMembers(MemberIds);
            CooperativeScore = Math.Max(0, CooperativeScore);

            if (!string.IsNullOrWhiteSpace(LeaderPlayerId) && !HasMember(LeaderPlayerId))
                MemberIds.Insert(0, LeaderPlayerId);

            Contributions ??= new List<GuildContributionEntry>();
            var normalizedContributions = new List<GuildContributionEntry>();
            var contributionIndex = new Dictionary<string, int>(StringComparer.Ordinal);

            for (var i = 0; i < Contributions.Count; i++)
            {
                var entry = Contributions[i];
                if (entry == null)
                    continue;

                entry.Normalize();
                if (string.IsNullOrWhiteSpace(entry.PlayerId) || !HasMember(entry.PlayerId))
                    continue;

                if (contributionIndex.TryGetValue(entry.PlayerId, out var existingIndex))
                {
                    var existing = normalizedContributions[existingIndex];
                    existing.Score = SafeAdd(existing.Score, entry.Score);
                }
                else
                {
                    contributionIndex.Add(entry.PlayerId, normalizedContributions.Count);
                    normalizedContributions.Add(entry);
                }
            }

            Contributions = normalizedContributions;

            ClaimedMilestones ??= new List<int>();
            var milestones = new List<int>();
            var seenMilestones = new HashSet<int>();
            for (var i = 0; i < ClaimedMilestones.Count; i++)
            {
                var milestone = ClaimedMilestones[i];
                if (milestone > 0 && seenMilestones.Add(milestone))
                    milestones.Add(milestone);
            }

            ClaimedMilestones = milestones;
        }

        public void AddContribution(string playerId, int amount)
        {
            if (string.IsNullOrWhiteSpace(playerId) || amount <= 0 || !HasMember(playerId))
                return;

            Contributions ??= new List<GuildContributionEntry>();

            for (var i = 0; i < Contributions.Count; i++)
            {
                var entry = Contributions[i];
                if (entry == null || entry.PlayerId != playerId)
                    continue;

                entry.Score = SafeAdd(entry.Score, amount);
                return;
            }

            Contributions.Add(new GuildContributionEntry
            {
                PlayerId = playerId.Trim(),
                Score = amount
            });
        }

        public bool MarkMilestoneClaimed(int milestone)
        {
            if (milestone <= 0)
                return false;

            ClaimedMilestones ??= new List<int>();
            if (ClaimedMilestones.Contains(milestone))
                return false;

            ClaimedMilestones.Add(milestone);
            return true;
        }

        private static int SafeAdd(int left, int right)
        {
            if (left >= int.MaxValue - right)
                return int.MaxValue;
            return left + right;
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
