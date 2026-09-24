using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.Collections
{
    public sealed class CollectionService
    {
        private readonly EconomyService economy = new();

        public bool TryComplete(CollectionSet set, CollectionState state, GameState gameState)
        {
            if (set == null || state == null || gameState == null)
                return false;

            if (string.IsNullOrWhiteSpace(set.Id) || state.HasCompleted(set.Id))
                return false;

            if (set.RequiredCards == null || set.RequiredCards.Count == 0)
                return false;

            for (var i = 0; i < set.RequiredCards.Count; i++)
            {
                if (!state.Owns(set.RequiredCards[i]))
                    return false;
            }

            state.CompletedSetIds.Add(set.Id);
            var granted = economy.GrantCoins(gameState, set.CompletionReward);
            gameState.Status.Set(NewMasterTextKeys.CollectionComplete, granted);
            return true;
        }

        public void AddCard(CollectionState state, string cardId)
        {
            if (state == null || string.IsNullOrWhiteSpace(cardId) || state.Owns(cardId))
                return;

            state.OwnedCardIds.Add(cardId);
        }
    }
}
