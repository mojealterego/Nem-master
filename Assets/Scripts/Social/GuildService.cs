namespace NewMaster.Social
{
    public sealed class GuildService
    {
        public bool TryCreate(GuildState guild, string guildId, string name, string leaderPlayerId)
        {
            if (guild == null ||
                string.IsNullOrWhiteSpace(guildId) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(leaderPlayerId))
                return false;

            guild.GuildId = guildId.Trim();
            guild.Name = name.Trim();
            guild.LeaderPlayerId = leaderPlayerId.Trim();
            guild.MemberIds.Clear();
            guild.MemberIds.Add(guild.LeaderPlayerId);
            guild.CooperativeScore = 0;
            return true;
        }

        public bool TryAddMember(GuildState guild, string playerId, int maxMembers = 50)
        {
            if (guild == null || string.IsNullOrWhiteSpace(playerId) ||
                maxMembers < 1 || guild.MemberIds.Count >= maxMembers ||
                guild.HasMember(playerId))
                return false;

            guild.MemberIds.Add(playerId.Trim());
            return true;
        }

        public bool TryRemoveMember(GuildState guild, string playerId)
        {
            if (guild == null || string.IsNullOrWhiteSpace(playerId) ||
                playerId == guild.LeaderPlayerId)
                return false;

            return guild.MemberIds.Remove(playerId);
        }

        public bool TryAddCooperativeScore(GuildState guild, int amount)
        {
            if (guild == null || amount <= 0)
                return false;

            var safeAmount = System.Math.Min(amount, int.MaxValue);
            guild.CooperativeScore = System.Math.Min(int.MaxValue - safeAmount, guild.CooperativeScore) + safeAmount;
            return true;
        }
    }
}
