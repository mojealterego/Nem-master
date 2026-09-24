using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace NewMaster.Localization
{
    public sealed class NewMasterLanguageController : MonoBehaviour
    {
        [SerializeField] private string defaultLocaleCode = "en";
        [SerializeField] private List<string> supportedLocaleCodes = new() { "en", "pl" };

        public IReadOnlyList<string> SupportedLocaleCodes => supportedLocaleCodes;
        public event Action<Locale> LocaleChanged;

        private void Awake()
        {
            LocalizationSettings.SelectedLocaleChanged += HandleLocaleChanged;
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= HandleLocaleChanged;
        }

        public void SetLanguage(string localeCode)
        {
            if (string.IsNullOrWhiteSpace(localeCode))
                return;

            var locale = LocalizationSettings.AvailableLocales.Locales
                .FirstOrDefault(x => string.Equals(x.Identifier.Code, localeCode, StringComparison.OrdinalIgnoreCase));

            if (locale == null)
                return;

            LocalizationSettings.SelectedLocale = locale;
        }

        public void SetLanguage(int index)
        {
            if (index < 0 || index >= supportedLocaleCodes.Count)
                return;

            SetLanguage(supportedLocaleCodes[index]);
        }

        private void Start()
        {
            if (LocalizationSettings.SelectedLocale == null)
                SetLanguage(defaultLocaleCode);
        }

        private void HandleLocaleChanged(Locale locale) =>
            LocaleChanged?.Invoke(locale);
    }
}
