using NUnit.Framework;
using UnityEngine;
using NewMaster.Worlds;

namespace NewMaster.Tests
{
    public sealed class WorldRuleServiceTests
    {
        [Test]
        public void WorldMetadataControlsBossAndSeasonalFlags()
        {
            var world = ScriptableObject.CreateInstance<WorldDefinition>();
            try
            {
                world.bossId = "boss.001";
                world.seasonalTag = "winter";
                var service = new WorldRuleService();
                Assert.That(service.HasBoss(world), Is.True);
                Assert.That(service.IsSeasonal(world), Is.True);
                Assert.That(service.ApplyRewardMultiplier(world, 100), Is.EqualTo(100));
            }
            finally { Object.DestroyImmediate(world); }
        }

        [Test]
        public void RewardMultiplierSaturatesAtLongMaxValue()
        {
            var world = ScriptableObject.CreateInstance<WorldDefinition>();
            try
            {
                world.rewardMultiplier = float.MaxValue;
                Assert.That(
                    new WorldRuleService().ApplyRewardMultiplier(world, long.MaxValue),
                    Is.EqualTo(long.MaxValue));
            }
            finally { Object.DestroyImmediate(world); }
        }
    }
}
