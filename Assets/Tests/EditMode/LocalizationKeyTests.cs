using System.Collections.Generic;
using NUnit.Framework;
using NewMaster.Localization;

namespace NewMaster.Tests
{
    public sealed class LocalizationKeyTests
    {
        [Test]
        public void LocalizationKeysRemainStable()
        {
            Assert.That(NewMasterTextKeys.GameReady, Is.EqualTo("game.ready"));
            Assert.That(NewMasterTextKeys.CoinsLabel, Is.EqualTo("hud.coins"));
            Assert.That(NewMasterTextKeys.RaidLoot, Is.EqualTo("raid.loot"));
        }

        [Test]
        public void LocalizationKeyWrapsStableValue()
        {
            var key = new LocalizationKey("test.key");
            Assert.That(key.Value, Is.EqualTo("test.key"));
            Assert.That(key.ToString(), Is.EqualTo("test.key"));
        }

        [Test]
        public void RuntimeLocalizationKeysAreUniqueAndNonEmpty()
        {
            var keys = new[]
            {
                NewMasterTextKeys.GameReady,
                NewMasterTextKeys.NoReward,
                NewMasterTextKeys.CoinsReward,
                NewMasterTextKeys.EnergyReward,
                NewMasterTextKeys.VillageProgress,
                NewMasterTextKeys.NewWorld,
                NewMasterTextKeys.VillageUpgraded,
                NewMasterTextKeys.RaidLoot,
                NewMasterTextKeys.RaidShieldBlocked,
                NewMasterTextKeys.RaidNoToken,
                NewMasterTextKeys.RaidEmptyTarget,
                NewMasterTextKeys.CollectionComplete,
                NewMasterTextKeys.CollectionIncomplete,
                NewMasterTextKeys.CoinsLabel,
                NewMasterTextKeys.EnergyLabel,
                NewMasterTextKeys.WorldLabel,
                NewMasterTextKeys.VillageLabel,
                NewMasterTextKeys.RaidTokensLabel,
                NewMasterTextKeys.RaidLootLabel,
                NewMasterTextKeys.RaidShieldsLabel,
                NewMasterTextKeys.CollectionCards,
                NewMasterTextKeys.CollectionReward
            };

            Assert.That(keys, Is.All.Not.Null.And.Not.Empty);
            Assert.That(new HashSet<string>(keys).Count, Is.EqualTo(keys.Length));
        }
    }
}
