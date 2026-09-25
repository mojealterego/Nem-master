#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NewMaster.Editor
{
    public static class NewMasterAndroidBuild
    {
        private const string ScenePath = "Assets/NewMaster/Scenes/NewMaster_Boot.unity";

        public static void BuildAndroidCi()
        {
            Build("Builds/NewMaster-Android.apk", false);
        }

        public static void BuildAndroidBundleCi()
        {
            Build("Builds/NewMaster-Android.aab", true);
        }

        private static void Build(string outputPath, bool appBundle)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            var previousBundleSetting = EditorUserBuildSettings.buildAppBundle;
            try
            {
                EditorUserBuildSettings.buildAppBundle = appBundle;
                var report = BuildPipeline.BuildPlayer(
                    new[] { ScenePath }, outputPath, BuildTarget.Android, BuildOptions.None);

                if (report.summary.result != BuildResult.Succeeded)
                    throw new BuildFailedException(
                        "New Master Android build failed: " + report.summary.result);

                Debug.Log("New Master Android build succeeded: " + outputPath);
            }
            finally
            {
                EditorUserBuildSettings.buildAppBundle = previousBundleSetting;
            }
        }
    }
}
