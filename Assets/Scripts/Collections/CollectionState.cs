using System;
using System.Collections.Generic;

namespace NewMaster.Collections
{
    [Serializable]
    public sealed class CollectionState
    {
        public List<string> OwnedCardIds = new();

        public bool Owns(string cardId) => OwnedCardIds.Contains(cardId);
    }
}
