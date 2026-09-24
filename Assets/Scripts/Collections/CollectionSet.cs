using System;
using System.Collections.Generic;

namespace NewMaster.Collections
{
    [Serializable]
    public sealed class CollectionSet
    {
        public string Id;
        public string DisplayName;
        public List<string> RequiredCards = new();
        public long CompletionReward;
    }
}
