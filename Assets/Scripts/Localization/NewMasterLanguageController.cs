using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace NewMaster.Localization
{
    public sealed class NewMasterLanguageController : MonoBehaviour
    {
        private const string SavedLocaleKey = "new_master.locale";

        [SerializeField] private string defaultLocaleCode = "en";
        [SerializeField] private List<string> supportedLocaleCodes = new() { "en", "pl" };

        public IReadOnlyList<string> SupportedLocaleCodes => supportedLocaleCodes;
        public string CurrentLocaleCode => LocalizationSettings.SelectedLocale?.Identifier.Code;
        public event Action<Locale> LocaleChanged;
        public event Action<string> LanguageCodeChanged;

        private void Awake()
        {
            LocalizationSettings.SelectedLocaleChanged += HandleLocaleChanged;
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= HandleLocaleChanged;
        }

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;

            var savedCode = PlayerPrefs.GetString(SavedLocaleKey, defaultLocaleCode);
            if (!SetLanguageInternal(savedCode))
                SetLanguageInternal(defaultLocaleCode);
        }

        public void SetLanguage(string localeCode)
        {
            if (SetLanguageInternal(localeCode))
                PlayerPrefs.Save();
        }

        public void SetLanguage(int index)
        {
            if (index < 0 || index >= supportedLocaleCodes.Count)
                return;

            SetLanguage(supportedLocaleCodes[index]);
        }

        private bool SetLanguageInternal(string localeCode)
        {
            if (string.IsNullOrWhiteSpace(localeCode))
                return false;

            if (!supportedLocaleCodes.Any(code =>
                    string.Equals(code, localeCode, StringComparison.OrdinalIgnoreCase)))
                return false;

            var locale = LocalizationSettings.AvailableLocales.Locales
                .FirstOrDefault(x => string.Equals(
                    x.Identifier.Code,
                    localeCode,
                    StringComparison.OrdinalIgnoreCase));

            if (locale == null)
                return false;

            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString(SavedLocaleKey, locale.Identifier.Code);
            return true;
        }

        public IReadOnlyList<string> GetSupportedLocaleLabels()
        {
            return supportedLocaleCodes
                .Select(code => LocalizationSettings.AvailableLocales.Locales
                    .FirstOrDefault(locale => string.Equals(
                        locale.Identifier.Code,
                        code,
                        StringComparison.OrdinalIgnoreCase))?.name ?? code)
                .ToList();
        }

        private void HandleLocaleChanged(Locale locale)
        {
            LocaleChanged?.Invoke(locale);
            LanguageCodeChanged?.Invoke(locale?.Identifier.Code);
        }
    }
}
