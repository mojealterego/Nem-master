using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

namespace NewMaster.Localization
{
    public static class NewMasterLocalization
    {
        private static readonly Dictionary<string, string> FallbackEnglish = new()
        {
            [NewMasterTextKeys.GameReady] = "New Master ready.",
            [NewMasterTextKeys.NoReward] = "No reward. The next spin can change everything.",
            [NewMasterTextKeys.CoinsReward] = "+{0} coins",
            [NewMasterTextKeys.EnergyReward] = "+{0} energy",
            [NewMasterTextKeys.VillageProgress] = "Village progress +1",
            [NewMasterTextKeys.NewWorld] = "New world: {0}",
            [NewMasterTextKeys.RaidLoot] = "+{0} loot",
            [NewMasterTextKeys.RaidShieldBlocked] = "Raid blocked by a shield.",
            [NewMasterTextKeys.RaidNoToken] = "No attack token.",
            [NewMasterTextKeys.RaidEmptyTarget] = "Target has no available loot.",
            [NewMasterTextKeys.CollectionComplete] = "Collection completed: +{0}",
            [NewMasterTextKeys.CollectionIncomplete] = "Collection is not complete yet.",
            [NewMasterTextKeys.CoinsLabel] = "Coins",
            [NewMasterTextKeys.EnergyLabel] = "Energy",
            [NewMasterTextKeys.WorldLabel] = "World {0:000}",
            [NewMasterTextKeys.VillageLabel] = "Village {0}",
            [NewMasterTextKeys.RaidTokensLabel] = "Attack tokens: {0}"
        };

        public static string Get(string key, params object[] args)
        {
            if (!LocalizationSettings.InitializationOperation.IsDone)
                return FallbackEnglish.TryGetValue(key, out var pendingFallback) ? string.Format(pendingFallback, args) : key;

            var localized = LocalizationSettings.StringDatabase.GetLocalizedString(key, args);

            if (!string.IsNullOrEmpty(localized))
                return localized;

            return FallbackEnglish.TryGetValue(key, out var fallback)
                ? string.Format(fallback, args)
                : key;
        }

        public static string Get(LocalizationKey key, params object[] args) =>
            Get(key.Value, args);
    }
}
