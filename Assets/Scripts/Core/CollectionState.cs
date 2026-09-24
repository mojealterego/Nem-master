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

        public bool HasCompleted(string setId) =>
            !string.IsNullOrWhiteSpace(setId) && CompletedSetIds.Contains(setId);

        public bool Owns(string cardId) =>
            !string.IsNullOrWhiteSpace(cardId) && OwnedCardIds.Contains(cardId);

        public bool OwnsPet(string petId) =>
            !string.IsNullOrWhiteSpace(petId) && OwnedPetIds.Contains(petId);

        public bool OwnsArtifact(string artifactId) =>
            !string.IsNullOrWhiteSpace(artifactId) && OwnedArtifactIds.Contains(artifactId);

        public void Normalize()
        {
            OwnedCardIds = NormalizeIds(OwnedCardIds);
            CompletedSetIds = NormalizeIds(CompletedSetIds);
            OwnedPetIds = NormalizeIds(OwnedPetIds);
            OwnedArtifactIds = NormalizeIds(OwnedArtifactIds);
        }

        private static List<string> NormalizeIds(List<string> source)
        {
            var normalized = new List<string>();
            if (source == null)
                return normalized;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < source.Count; i++)
            {
                var value = source[i];
                if (string.IsNullOrWhiteSpace(value) || !seen.Add(value))
                    continue;

                normalized.Add(value);
            }

            return normalized;
        }
    }
}
