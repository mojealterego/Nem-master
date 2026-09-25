using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using NewMaster.Core;

namespace NewMaster.Localization
{
    public static class NewMasterLocalization
    {
        private static readonly Dictionary<string, string> FallbackEnglish = new()
        {
            [NewMasterTextKeys.GameReady] = "New Master ready.",
            [NewMasterTextKeys.NoReward] = "No reward. The next spin can change everything.",
            [NewMasterTextKeys.CoinsReward] = "+{0:N0} coins",
            [NewMasterTextKeys.EnergyReward] = "+{0} energy",
            [NewMasterTextKeys.VillageProgress] = "Village progress +1",
            [NewMasterTextKeys.JackpotReward] = "JACKPOT! +{0:N0} coins",
            [NewMasterTextKeys.DailyReward] = "Daily reward day {0}: +{1:N0} coins +{2} energy",
            [NewMasterTextKeys.NewWorld] = "New world: {0:000}",
            [NewMasterTextKeys.VillageUpgraded] = "Building {1}: level {0}",
            [NewMasterTextKeys.RaidLoot] = "+{0:N0} loot",
            [NewMasterTextKeys.RaidShieldBlocked] = "Raid blocked by a shield.",
            [NewMasterTextKeys.RaidNoToken] = "No attack token.",
            [NewMasterTextKeys.RaidEmptyTarget] = "Target has no available loot.",
            [NewMasterTextKeys.RaidDetailed] = "+{0:N0} loot · defense {1}% · village damage {2}",
            [NewMasterTextKeys.VillageRaidDamage] = "Village damage +{0}",
            [NewMasterTextKeys.CounterAttackReady] = "Counter-attack ready: {0:N0} bounty",
            [NewMasterTextKeys.WorldBossAttack] = "Boss damage {0:N0} · {1:N0} HP remaining",
            [NewMasterTextKeys.WorldBossDefeated] = "WORLD BOSS DEFEATED",
            [NewMasterTextKeys.WorldBossClaimed] = "Boss reward: +{0:N0} coins",
            [NewMasterTextKeys.CollectionComplete] = "Collection completed: +{0:N0}",
            [NewMasterTextKeys.CollectionIncomplete] = "Collection is not complete yet.",
            [NewMasterTextKeys.CoinsLabel] = "Coins",
            [NewMasterTextKeys.EnergyLabel] = "Energy",
            [NewMasterTextKeys.WorldLabel] = "World {0:000}",
            [NewMasterTextKeys.VillageLabel] = "Village {0}",
            [NewMasterTextKeys.RaidTokensLabel] = "Attack tokens: {0}",
            [NewMasterTextKeys.RaidLootLabel] = "Loot: {0:N0}",
            [NewMasterTextKeys.RaidShieldsLabel] = "Shields: {0}",
            [NewMasterTextKeys.CollectionCards] = "{0}/{1} cards",
            [NewMasterTextKeys.CollectionReward] = "Reward: {0:N0} coins",
            [NewMasterTextKeys.LiveOpsNoEvent] = "No active event",
            [NewMasterTextKeys.LiveOpsMissionProgress] = "{0} · {1:N0}/{2:N0}",
            [NewMasterTextKeys.LiveOpsMissionClaimed] = "{0} · CLAIMED",
            ["world.001"] = "World 001",
            ["building.main"] = "Main Building",
            ["event.starter"] = "Starter Event",
            ["mission.spin_50"] = "Spin 50 times",
            ["mission.spin_250"] = "Spin 250 times",
            ["mission.jackpot_10"] = "Hit 10 jackpots"
        };

        public static string Get(string key, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            if (!LocalizationSettings.InitializationOperation.IsDone)
                return GetFallback(key, args);

            var localized = LocalizationSettings.StringDatabase.GetLocalizedString(key, args);
            return !string.IsNullOrEmpty(localized) ? localized : GetFallback(key, args);
        }

        public static string Get(LocalizationKey key, params object[] args) =>
            Get(key.Value, args);

        public static string Get(GameStatus status)
        {
            if (status == null)
                return Get(NewMasterTextKeys.GameReady);

            return status.Key switch
            {
                NewMasterTextKeys.VillageUpgraded =>
                    Get(status.Key, status.Amount, Get(status.Context)),
                NewMasterTextKeys.NewWorld =>
                    Get(status.Key, Get(status.Context)),
                NewMasterTextKeys.DailyReward =>
                    Get(status.Key, status.Amount, status.SecondaryValue, ParseContextInt(status.Context)),
                NewMasterTextKeys.RaidDetailed =>
                    Get(status.Key, status.Amount, status.SecondaryValue, ParseContextInt(status.Context)),
                NewMasterTextKeys.WorldBossAttack =>
                    Get(status.Key, status.Amount, status.SecondaryValue),
                _ => Get(status.Key, status.Amount)
            };
        }

        private static int ParseContextInt(string value)
        {
            return int.TryParse(
                value,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out var parsed)
                ? parsed
                : 0;
        }

        private static string GetFallback(string key, object[] args)
        {
            if (FallbackEnglish.TryGetValue(key, out var fallback))
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, fallback, args);

            if (key.StartsWith("world.", System.StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(key.Substring("world.".Length), out var worldId))
            {
                return $"World {worldId:000}";
            }

            return key;
        }
    }
}
