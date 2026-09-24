using NUnit.Framework;
using NewMaster.Village;

namespace NewMaster.Tests.EditMode
{
    public sealed class VillageDefenseServiceTests
    {
        [Test]
        public void DefenseScoreUsesLevelAndDamage()
        {
            var village = new VillageState();
            village.Buildings.Add(new VillageState.BuildingProgress
            {
                BuildingId = 1,
                Level = 3,
                Damage = 50,
                HasDefense = true
            });

            Assert.AreEqual(250, new VillageDefenseService().GetDefenseScore(village));
        }

        [Test]
        public void RaidDamageOnlyHitsDefendedBuildings()
        {
            var village = new VillageState();
            village.Buildings.Add(new VillageState.BuildingProgress
            {
                BuildingId = 1,
                Level = 2,
                HasDefense = false
            });
            village.Buildings.Add(new VillageState.BuildingProgress
            {
                BuildingId = 2,
                Level = 2,
                HasDefense = true
            });

            Assert.AreEqual(75, new VillageDefenseService().ApplyRaidDamage(village, 75));
            Assert.AreEqual(0, village.GetDamage(1));
            Assert.AreEqual(75, village.GetDamage(2));
        }

        [Test]
        public void NegativeDamageIsRejected()
        {
            var village = new VillageState();
            Assert.AreEqual(0, new VillageDefenseService().ApplyRaidDamage(village, -10));
        }
    }
}
