using UnityEngine;

namespace NewMaster.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class NewMasterSafeArea : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Apply();
        }

        private void OnEnable() => Apply();

        private void Update()
        {
            if (Screen.safeArea != lastSafeArea ||
                Screen.width != lastScreenSize.x ||
                Screen.height != lastScreenSize.y)
            {
                Apply();
            }
        }

        private void Apply()
        {
            if (rectTransform == null)
                return;

            var safeArea = Screen.safeArea;
            lastSafeArea = safeArea;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
