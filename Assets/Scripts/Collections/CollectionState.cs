using System;
using System.Collections.Generic;

namespace NewMaster.Collections
{
    [Serializable]
    public sealed class CollectionState
    {
        public List<string> OwnedCardIds = new();
        public List<string> CompletedSetIds = new();

        public bool HasCompleted(string setId) => CompletedSetIds.Contains(setId);

        public bool Owns(string cardId) => OwnedCardIds.Contains(cardId);
    }
}
