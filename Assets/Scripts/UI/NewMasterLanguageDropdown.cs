using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NewMaster.Localization;

namespace NewMaster.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public sealed class NewMasterLanguageDropdown : MonoBehaviour
    {
        [SerializeField] private NewMasterLanguageController controller;

        private TMP_Dropdown dropdown;
        private List<string> localeCodes = new();

        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            if (controller == null)
                controller = FindFirstObjectByType<NewMasterLanguageController>();
        }

        private IEnumerator Start()
        {
            if (controller == null)
                yield break;

            if (!controller.IsReady)
                yield return new WaitUntil(() => controller.IsReady);

            RebuildOptions();
            controller.LanguageCodeChanged += HandleLanguageChanged;
            dropdown.onValueChanged.AddListener(SelectLanguage);
        }

        private void OnDestroy()
        {
            if (controller != null)
                controller.LanguageCodeChanged -= HandleLanguageChanged;

            if (dropdown != null)
                dropdown.onValueChanged.RemoveListener(SelectLanguage);
        }

        private void RebuildOptions()
        {
            localeCodes = new List<string>(controller.SupportedLocaleCodes);
            var labels = new List<string>(controller.GetSupportedLocaleLabels());

            dropdown.ClearOptions();
            dropdown.AddOptions(labels);

            var selected = localeCodes.FindIndex(code =>
                string.Equals(code, controller.CurrentLocaleCode, System.StringComparison.OrdinalIgnoreCase));

            dropdown.SetValueWithoutNotify(selected < 0 ? 0 : selected);
        }

        private void SelectLanguage(int index)
        {
            if (index < 0 || index >= localeCodes.Count)
                return;

            controller.SetLanguage(localeCodes[index]);
        }

        private void HandleLanguageChanged(string localeCode)
        {
            var index = localeCodes.FindIndex(code =>
                string.Equals(code, localeCode, System.StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
                dropdown.SetValueWithoutNotify(index);
        }
    }
}
