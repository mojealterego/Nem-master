using NUnit.Framework;
using NewMaster.Collections;
using System.Collections.Generic;

namespace NewMaster.Tests
{
    public sealed class CollectionStateTests
    {
        [Test]
        public void NormalizeRemovesBlankAndDuplicateIds()
        {
            var state = new CollectionState
            {
                OwnedCardIds = new List<string> { "card-a", "", "card-a", "card-b" },
                CompletedSetIds = new List<string> { "set-a", "set-a" },
                OwnedPetIds = null,
                OwnedArtifactIds = new List<string> { "artifact-a", " " }
            };

            state.Normalize();

            Assert.That(state.OwnedCardIds, Is.EqualTo(new[] { "card-a", "card-b" }));
            Assert.That(state.CompletedSetIds, Is.EqualTo(new[] { "set-a" }));
            Assert.That(state.OwnedPetIds, Is.Empty);
            Assert.That(state.OwnedArtifactIds, Is.EqualTo(new[] { "artifact-a" }));
        }
    }
}
