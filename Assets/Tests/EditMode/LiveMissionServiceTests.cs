using NUnit.Framework;
using NewMaster.Core;
using NewMaster.LiveOps;
using UnityEngine;

namespace NewMaster.Tests
{
    public sealed class LiveMissionServiceTests
    {
        [Test]
        public void MissionProgressCanBeClaimedOnce()
        {
            var definition = ScriptableObject.CreateInstance<LiveMissionDefinition>();
            definition.missionId = "spin-10";
            definition.target = 10;
            definition.coinReward = 500;
            definition.energyReward = 2;

            var state = new LiveMissionState();
            var gameState = new GameState();

            try
            {
                var service = new LiveMissionService();
                Assert.That(service.AddProgress(definition, state, 6), Is.True);
                Assert.That(service.AddProgress(definition, state, 8), Is.True);
                Assert.That(state.Progress, Is.EqualTo(10));
                Assert.That(service.TryClaim(definition, state, gameState), Is.True);
                Assert.That(gameState.Coins, Is.EqualTo(500));
                Assert.That(gameState.Energy, Is.EqualTo(12));
                Assert.That(service.TryClaim(definition, state, gameState), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }
    }
}
