using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewMaster.UI
{
    /// <summary>
    /// Owns the product-brand presentation layer for the boot scene.
    /// It does not own gameplay state; it only projects the configured brand assets into uGUI.
    /// </summary>
    public sealed class NewMasterBranding : MonoBehaviour
    {
        [SerializeField] private Image logoImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subtitleText;
        [SerializeField] private Sprite logo;

        [SerializeField] private string productName = "NEW MASTER";
        [SerializeField] private string subtitle = "KINGDOMS • SPINS • RAIDS • COLLECTIONS";

        private void Awake()
        {
            Apply();
        }

        private void OnEnable()
        {
            Apply();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Apply();
        }
#endif

        public void Apply()
        {
            if (logoImage != null)
                logoImage.sprite = logo;

            if (titleText != null)
                titleText.text = productName;

            if (subtitleText != null)
                subtitleText.text = subtitle;
        }
    }
}
