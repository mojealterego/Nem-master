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
            const string outputPath = "Builds/NewMaster-Android.apk";
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            var report = BuildPipeline.BuildPlayer(
                new[] { ScenePath }, outputPath, BuildTarget.Android, BuildOptions.None);

            if (report.summary.result != BuildResult.Succeeded)
                throw new BuildFailedException("New Master Android build failed: " + report.summary.result);

            Debug.Log("New Master Android build succeeded: " + outputPath);
        }
    }
}