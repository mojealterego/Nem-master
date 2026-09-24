#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NewMaster.Worlds;

namespace NewMaster.Editor
{
    public static class NewMasterWorldCatalogGenerator
    {
        private const int WorldCount = 365;
        private const string RootFolder = "Assets/NewMaster/Content/Worlds";
        private const string CatalogPath = RootFolder + "/WorldCatalog.asset";

        [MenuItem("New Master/Content/Generate 365 Worlds")]
        public static void Generate()
        {
            EnsureFolder("Assets/NewMaster");
            EnsureFolder("Assets/NewMaster/Content");
            EnsureFolder(RootFolder);

            var worlds = new List<WorldDefinition>(WorldCount);

            for (var id = 1; id <= WorldCount; id++)
            {
                var path = $"{RootFolder}/World_{id:000}.asset";
                var world = AssetDatabase.LoadAssetAtPath<WorldDefinition>(path);

                if (world == null)
                {
                    world = ScriptableObject.CreateInstance<WorldDefinition>();
                    AssetDatabase.CreateAsset(world, path);
                }

                world.id = id;
                world.worldName = $"World {id:000}";
                world.localizationKey = $"world.{id:000}";
                world.rewardMultiplier = Mathf.Pow(1.055f, id - 1);
                world.unlockCost = id == 1 ? 0 : CalculateUnlockCost(id);
                world.baseSpinReward = CalculateBaseReward(id);
                world.energyReward = id % 15 == 0 ? 2 : 1;
                world.bossId = id % 10 == 0 ? $"boss.{id:000}" : string.Empty;
                world.seasonalTag = id % 20 == 0 ? "seasonal" : string.Empty;
                world.themeColor = Color.HSVToRGB(Mathf.Repeat((id - 1) * 0.073f, 1f), 0.42f, 0.92f);
                world.symbols = CreateSymbols(id);

                EditorUtility.SetDirty(world);
                worlds.Add(world);
            }

            var catalog = AssetDatabase.LoadAssetAtPath<WorldCatalog>(CatalogPath);
            if (catalog == null)
            {
                catalog = ScriptableObject.CreateInstance<WorldCatalog>();
                AssetDatabase.CreateAsset(catalog, CatalogPath);
            }

            var serialized = new SerializedObject(catalog);
            var property = serialized.FindProperty("worlds");
            property.arraySize = worlds.Count;

            for (var i = 0; i < worlds.Count; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = worlds[i];

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(catalog);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"New Master: generated {WorldCount} world definitions and catalog.");
        }

        private static long CalculateUnlockCost(int id)
        {
            var raw = 1000d * Mathf.Pow(1.12f, id - 1);
            return (long)Mathf.Min((float)raw, 9_000_000_000f);
        }

        private static long CalculateBaseReward(int id)
        {
            var raw = 100d * Mathf.Pow(1.06f, id - 1);
            return (long)Mathf.Min((float)raw, 2_000_000_000f);
        }

        private static List<string> CreateSymbols(int id)
        {
            var variants = new[]
            {
                new[] { "Coin", "Crown", "Chest", "Energy", "Hammer" },
                new[] { "Coin", "Gem", "Chest", "Energy", "Hammer" },
                new[] { "Coin", "Star", "Chest", "Energy", "Hammer" },
                new[] { "Coin", "Relic", "Chest", "Energy", "Hammer" }
            };

            var source = variants[(id - 1) % variants.Length];
            return new List<string>(source);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parent = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/");
            var folder = System.IO.Path.GetFileName(path);

            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent, folder);
        }
    }
}
#endif
