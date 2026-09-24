#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using NewMaster.LiveOps;

namespace NewMaster.Editor
{
    public static class NewMasterLiveOpsContentGenerator
    {
        private const string Root = "Assets/NewMaster/LiveOps";

        [MenuItem("New Master/Content/Generate LiveOps Starter")]
        public static void GenerateStarter()
        {
            EnsureFolder(Root);

            var now = DateTime.UtcNow;
            var eventDefinition = CreateEvent(
                $"{Root}/Event_Starter.asset",
                "event.starter",
                "event.starter",
                now,
                now.AddDays(7));

            CreateMission(
                $"{Root}/Mission_Spin50.asset",
                "spin.50",
                "mission.spin_50",
                50,
                5000,
                2,
                eventDefinition.eventId);

            CreateMission(
                $"{Root}/Mission_Spin250.asset",
                "spin.250",
                "mission.spin_250",
                250,
                25000,
                5,
                eventDefinition.eventId);

            CreateMission(
                $"{Root}/Mission_Jackpot10.asset",
                "jackpot.10",
                "mission.jackpot_10",
                10,
                50000,
                10,
                eventDefinition.eventId);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("New Master LiveOps starter content generated.");
        }

        private static LiveEventDefinition CreateEvent(
            string path,
            string eventId,
            string localizationKey,
            DateTime start,
            DateTime end)
        {
            var asset = AssetDatabase.LoadAssetAtPath<LiveEventDefinition>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<LiveEventDefinition>();
                AssetDatabase.CreateAsset(asset, path);
            }

            asset.eventId = eventId;
            asset.localizationKey = localizationKey;
            asset.SetWindowUtc(start, end);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static void CreateMission(
            string path,
            string missionId,
            string localizationKey,
            int target,
            long coins,
            int energy,
            string eventId)
        {
            var asset = AssetDatabase.LoadAssetAtPath<LiveMissionDefinition>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<LiveMissionDefinition>();
                AssetDatabase.CreateAsset(asset, path);
            }

            asset.missionId = missionId;
            asset.localizationKey = localizationKey;
            asset.target = target;
            asset.coinReward = coins;
            asset.energyReward = energy;
            asset.eventId = eventId;
            EditorUtility.SetDirty(asset);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
            var name = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent ?? "Assets", name);
        }
    }
}
#endif
