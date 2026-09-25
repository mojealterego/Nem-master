#if UNITY_EDITOR
using System;
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
        public const string IconPath = "Assets/NewMaster/Branding/NewMasterIcon.jpg";
        private const string ProductName = "New Master";

        [MenuItem("New Master/Branding/Apply Android App Icon")]
        public static void ApplyAndroidAppIcon()
        {
            ApplyAndroidAppIconOrThrow();
            Debug.Log("New Master Android branding applied.");
        }

        public static void ApplyAndroidAppIconOrThrow()
        {
            var icon = LoadValidatedIcon();
            var buildTarget = NamedBuildTarget.Android;
            var kind = IconKind.Application;
            var sizes = PlayerSettings.GetIconSizes(buildTarget, kind);

            if (sizes == null || sizes.Length == 0)
                throw new InvalidOperationException("Unity returned no Android application icon slots.");

            var icons = new Texture2D[sizes.Length];
            for (var i = 0; i < icons.Length; i++)
                icons[i] = icon;

            PlayerSettings.SetIcons(buildTarget, icons, kind);
            PlayerSettings.productName = ProductName;
            AssetDatabase.SaveAssets();

            Debug.Log($"New Master branding applied to {sizes.Length} Android application icon slots.");
        }

        [MenuItem("New Master/Branding/Validate Brand Asset")]
        public static void ValidateBrandAsset()
        {
            try
            {
                var icon = LoadValidatedIcon();
                Debug.Log($"New Master branding asset: {icon.width}x{icon.height}, format={icon.format}.");
            }
            catch (Exception exception)
            {
                Debug.LogError("New Master branding validation failed: " + exception.Message);
            }
        }

        public static void ValidateBrandAssetOrThrow()
        {
            _ = LoadValidatedIcon();
        }

        private static Texture2D LoadValidatedIcon()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (icon == null)
                throw new InvalidOperationException("New Master icon not found at " + IconPath);

            if (icon.width != icon.height)
                throw new InvalidOperationException(
                    $"New Master icon must be square, but is {icon.width}x{icon.height}.");

            if (icon.width < 512)
                throw new InvalidOperationException(
                    $"New Master icon must be at least 512x512, but is {icon.width}x{icon.height}.");

            return icon;
        }
    }
}
#endif
