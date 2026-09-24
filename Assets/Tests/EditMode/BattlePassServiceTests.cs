using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Monetization;
using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Tests
{
    public sealed class BattlePassServiceTests
    {
        [Test]
        public void ExperienceProgressionIsClamped()
        {
            var state = new BattlePassState { Experience = int.MaxValue - 5 };

            var added = new BattlePassService().AddExperience(state, 100);

            Assert.That(added, Is.EqualTo(5));
            Assert.That(state.Experience, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void UnlockedTierCanBeClaimedOnce()
        {
            var definition = ScriptableObject.CreateInstance<BattlePassDefinition>();
            definition.seasonId = "season-1";
            definition.tiers = new List<BattlePassDefinition.Tier>
            {
                new BattlePassDefinition.Tier
                {
                    level = 1,
                    experienceRequired = 100,
                    coinReward = 500,
                    energyReward = 2
                }
            };

            var state = new BattlePassState { SeasonId = "season-1", Experience = 100 };
            var gameState = new GameState();
            var service = new BattlePassService();

            try
            {
                Assert.That(service.TryClaimNext(definition, state, gameState), Is.True);
                Assert.That(gameState.Coins, Is.EqualTo(500));
                Assert.That(gameState.Energy, Is.EqualTo(12));
                Assert.That(service.TryClaimNext(definition, state, gameState), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }
    }
}
