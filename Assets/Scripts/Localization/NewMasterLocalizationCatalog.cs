using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Localization
{
    [CreateAssetMenu(fileName = "NewMasterLocalizationCatalog", menuName = "New Master/Localization Catalog")]
    public sealed class NewMasterLocalizationCatalog : ScriptableObject
    {
        [SerializeField] private List<string> localeCodes = new() { "en", "pl" };

        public IReadOnlyList<string> LocaleCodes => localeCodes;

        public bool Supports(string localeCode) =>
            !string.IsNullOrWhiteSpace(localeCode) && localeCodes.Contains(localeCode);
    }
}
