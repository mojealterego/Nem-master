using System;
using System.Collections.Generic;

namespace NewMaster.Collections
{
    [Serializable]
    public sealed class CollectionState
    {
        public List<string> OwnedCardIds = new();
        public List<string> CompletedSetIds = new();
        public List<string> OwnedPetIds = new();
        public List<string> OwnedArtifactIds = new();

        public bool HasCompleted(string setId) => !string.IsNullOrWhiteSpace(setId) && CompletedSetIds.Contains(setId);
        public bool Owns(string cardId) => !string.IsNullOrWhiteSpace(cardId) && OwnedCardIds.Contains(cardId);
        public bool OwnsPet(string petId) => !string.IsNullOrWhiteSpace(petId) && OwnedPetIds.Contains(petId);
        public bool OwnsArtifact(string artifactId) => !string.IsNullOrWhiteSpace(artifactId) && OwnedArtifactIds.Contains(artifactId);
    }
}
