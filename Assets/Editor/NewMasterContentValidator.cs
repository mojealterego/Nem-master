#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NewMaster.Worlds;

namespace NewMaster.Editor
{
    public static class NewMasterContentValidator
    {
        private const int ExpectedWorldCount = 365;
        private const string WorldFolder = "Assets/NewMaster/Content/Worlds";
        private const string CatalogPath = WorldFolder + "/WorldCatalog.asset";

        [MenuItem("New Master/Content/Validate World Catalog")]
        public static void ValidateWorldCatalog()
        {
            var guids = AssetDatabase.FindAssets("t:WorldDefinition", new[] { WorldFolder });
            var ids = new HashSet<int>();
            var errors = 0;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var world = AssetDatabase.LoadAssetAtPath<WorldDefinition>(path);

                if (world == null)
                    continue;

                if (!ids.Add(world.id))
                {
                    Debug.LogError($"New Master: duplicate world id {world.id}: {path}");
                    errors++;
                }

                if (world.id < 1 || world.id > ExpectedWorldCount)
                {
                    Debug.LogError($"New Master: world id out of range ({world.id}): {path}");
                    errors++;
                }

                if (string.IsNullOrWhiteSpace(world.localizationKey))
                {
                    Debug.LogError($"New Master: missing localization key: {path}");
                    errors++;
                }

                if (world.symbols == null || world.symbols.Count < 3)
                {
                    Debug.LogError($"New Master: world {world.id} has fewer than 3 symbols: {path}");
                    errors++;
                }
            }

            for (var id = 1; id <= ExpectedWorldCount; id++)
            {
                if (!ids.Contains(id))
                {
                    Debug.LogError($"New Master: missing world definition {id:000}.");
                    errors++;
                }
            }

            var catalog = AssetDatabase.LoadAssetAtPath<WorldCatalog>(CatalogPath);
            if (catalog == null)
            {
                Debug.LogError($"New Master: missing catalog asset: {CatalogPath}");
                errors++;
            }
            else
            {
                if (catalog.Worlds == null || catalog.Worlds.Count != ExpectedWorldCount)
                {
                    var actual = catalog.Worlds == null ? 0 : catalog.Worlds.Count;
                    Debug.LogError($"New Master: catalog contains {actual} entries; expected {ExpectedWorldCount}.");
                    errors++;
                }

                var catalogIds = new HashSet<int>();
                foreach (var world in catalog.Worlds)
                {
                    if (world == null)
                    {
                        Debug.LogError("New Master: catalog contains a null world reference.");
                        errors++;
                        continue;
                    }

                    if (!catalogIds.Add(world.id))
                    {
                        Debug.LogError($"New Master: catalog contains duplicate world id {world.id}.");
                        errors++;
                    }
                }
            }

            try
            {
                NewMasterBrandingEditor.ValidateBrandAssetOrThrow();
            }
            catch (Exception exception)
            {
                Debug.LogError("New Master: branding validation failed: " + exception.Message);
                errors++;
            }

            if (errors == 0 && ids.Count == ExpectedWorldCount)
                Debug.Log($"New Master: content validation passed ({ExpectedWorldCount} worlds + branding).");
            else
                Debug.LogError($"New Master: content validation found {errors} issue(s).");
        }
    }
}
#endif
