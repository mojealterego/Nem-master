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
            [NewMasterTextKeys.NewWorld] = "New world: {0:000}",
            [NewMasterTextKeys.VillageUpgraded] = "Building {1}: level {0}",
            [NewMasterTextKeys.RaidLoot] = "+{0:N0} loot",
            [NewMasterTextKeys.RaidShieldBlocked] = "Raid blocked by a shield.",
            [NewMasterTextKeys.RaidNoToken] = "No attack token.",
            [NewMasterTextKeys.RaidEmptyTarget] = "Target has no available loot.",
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
            ["world.001"] = "World 001",
            ["building.main"] = "Main Building"
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
                _ => Get(status.Key, status.Amount)
            };
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
