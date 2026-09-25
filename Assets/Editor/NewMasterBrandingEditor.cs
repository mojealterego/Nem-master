#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace NewMaster.Editor
{
    /// <summary>
    /// Applies the approved New Master product identity to Unity's Android application icon slots.
    /// The operation is explicit and editor-only so PlayerSettings are not mutated during runtime.
    /// </summary>
    public static class NewMasterBrandingEditor
    {
        private const string IconPath = "Assets/NewMaster/Branding/NewMasterIcon.jpg";

        [MenuItem("New Master/Branding/Apply Android App Icon")]
        public static void ApplyAndroidAppIcon()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (icon == null)
                throw new System.InvalidOperationException("New Master icon not found at " + IconPath);

            var buildTarget = NamedBuildTarget.Android;
            var kind = IconKind.Application;
            var sizes = PlayerSettings.GetIconSizes(buildTarget, kind);

            if (sizes == null || sizes.Length == 0)
                throw new System.InvalidOperationException("Unity returned no Android application icon slots.");

            var icons = new Texture2D[sizes.Length];
            for (var i = 0; i < icons.Length; i++)
                icons[i] = icon;

            PlayerSettings.SetIcons(buildTarget, icons, kind);
            PlayerSettings.productName = "New Master";

            AssetDatabase.SaveAssets();
            Debug.Log($"New Master branding applied: {sizes.Length} Android application icon slots.");
        }

        [MenuItem("New Master/Branding/Validate Brand Asset")]
        public static void ValidateBrandAsset()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (icon == null)
            {
                Debug.LogError("New Master branding validation failed: missing " + IconPath);
                return;
            }

            if (icon.width != icon.height)
                Debug.LogError($"New Master branding validation failed: icon is {icon.width}x{icon.height}, expected square.");

            Debug.Log($"New Master branding asset: {icon.width}x{icon.height}, format={icon.format}.");
        }
    }
}
#endif
