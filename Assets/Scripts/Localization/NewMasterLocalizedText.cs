using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace NewMaster.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class NewMasterLocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private bool refreshOnEnable = true;

        private LocalizeStringEvent localizeEvent;
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            localizeEvent = GetComponent<LocalizeStringEvent>();

            if (localizeEvent == null)
                localizeEvent = gameObject.AddComponent<LocalizeStringEvent>();
        }

        private void OnEnable()
        {
            if (refreshOnEnable)
                Refresh();
        }

        public void SetKey(string localizationKey)
        {
            key = localizationKey;
            Refresh();
        }

        public void Refresh()
        {
            if (localizeEvent == null)
                return;

            localizeEvent.StringReference = new LocalizedString
            {
                TableReference = "New Master UI",
                TableEntryReference = key
            };

            localizeEvent.OnUpdateString.RemoveListener(Apply);
            localizeEvent.OnUpdateString.AddListener(Apply);
            localizeEvent.RefreshString();
        }

        private void Apply(string value)
        {
            if (text != null)
                text.text = value;
        }
    }
}
