using NUnit.Framework;
using NewMaster.Collections;

namespace NewMaster.Tests
{
    public sealed class CollectionCompanionStateTests
    {
        [Test]
        public void CompanionOwnershipIsTrackedSeparately()
        {
            var state = new CollectionState();
            state.OwnedPetIds.Add("pet.001");
            state.OwnedArtifactIds.Add("artifact.001");

            Assert.That(state.OwnsPet("pet.001"), Is.True);
            Assert.That(state.OwnsArtifact("artifact.001"), Is.True);
            Assert.That(state.Owns("pet.001"), Is.False);
        }
    }
}
