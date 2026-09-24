namespace NewMaster.Social
{
    public sealed class SocialService
    {
        public bool TryAddFriend(SocialState state, string playerId)
        {
            if (state == null || string.IsNullOrWhiteSpace(playerId) ||
                playerId == state.PlayerId || state.IsBlocked(playerId) ||
                state.FriendIds.Contains(playerId))
                return false;

            state.FriendIds.Add(playerId);
            return true;
        }

        public bool TryRemoveFriend(SocialState state, string playerId)
        {
            return state != null && !string.IsNullOrWhiteSpace(playerId) &&
                   state.FriendIds.Remove(playerId);
        }

        public bool TryBlock(SocialState state, string playerId)
        {
            if (state == null || string.IsNullOrWhiteSpace(playerId) || playerId == state.PlayerId)
                return false;

            state.FriendIds.Remove(playerId);
            if (state.IsBlocked(playerId))
                return false;

            state.BlockedPlayerIds.Add(playerId);
            return true;
        }
    }
}
