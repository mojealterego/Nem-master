using NUnit.Framework;
using NewMaster.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Tests
{
    public sealed class CollectionCompanionServiceTests
    {
        [Test]
        public void OwnedCompanionProvidesHighestConfiguredBonus()
        {
            var pet = ScriptableObject.CreateInstance<CompanionDefinition>();
            var artifact = ScriptableObject.CreateInstance<CompanionDefinition>();

            pet.Id = "pet-1";
            pet.Type = CompanionType.Pet;
            pet.RewardMultiplier = 1.25f;
            pet.EnergyBonus = 2;

            artifact.Id = "artifact-1";
            artifact.Type = CompanionType.Artifact;
            artifact.RewardMultiplier = 1.5f;
            artifact.EnergyBonus = 4;

            var state = new CollectionState
            {
                OwnedPetIds = new List<string> { "pet-1" },
                OwnedArtifactIds = new List<string> { "artifact-1" }
            };

            try
            {
                var service = new CollectionCompanionService();
                Assert.That(service.GetRewardMultiplier(new[] { pet, artifact }, state), Is.EqualTo(1.5f));
                Assert.That(service.GetEnergyBonus(new[] { pet, artifact }, state), Is.EqualTo(4));
            }
            finally
            {
                Object.DestroyImmediate(pet);
                Object.DestroyImmediate(artifact);
            }
        }
    }
}
