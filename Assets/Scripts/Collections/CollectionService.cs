using System.Collections.Generic;
using NewMaster.Core;

namespace NewMaster.Collections
{
    public sealed class CollectionService
    {
        public bool TryComplete(CollectionSet set, CollectionState state, GameState gameState)
        {
            if (set == null || state == null || gameState == null)
                return false;

            if (set.RequiredCards == null || set.RequiredCards.Count == 0)
                return false;

            for (var i = 0; i < set.RequiredCards.Count; i++)
            {
                if (!state.Owns(set.RequiredCards[i]))
                    return false;
            }

            gameState.Coins += set.CompletionReward;
            gameState.StatusMessage = $"Kolekcja ukończona: {set.DisplayName}";
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
