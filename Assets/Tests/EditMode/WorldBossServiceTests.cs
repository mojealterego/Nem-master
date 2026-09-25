using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class WorldBossServiceTests
    {
        [Test]
        public void EnsureBossInitializesNewBoss()
        {
            var state = new WorldBossState();
            var service = new WorldBossService();

            Assert.IsTrue(service.EnsureBoss(state, "boss.010", 10000));
            Assert.AreEqual("boss.010", state.BossId);
            Assert.AreEqual(10000, state.Health);
        }

        [Test]
        public void AttackClampsAtZeroAndMarksDefeated()
        {
            var state = new WorldBossState();
            state.Initialize("boss.010", 1000);

            var applied = new WorldBossService().Attack(state, 1500);

            Assert.AreEqual(1000, applied);
            Assert.AreEqual(0, state.Health);
            Assert.IsTrue(state.Defeated);
        }

        [Test]
        public void CannotAttackDefeatedBoss()
        {
            var state = new WorldBossState();
            state.Initialize("boss.010", 1000);
            state.Defeated = true;

            Assert.AreEqual(0, new WorldBossService().Attack(state, 100));
        }
    }
}
