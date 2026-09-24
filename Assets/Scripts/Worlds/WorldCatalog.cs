using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.Worlds
{
    [CreateAssetMenu(fileName = "WorldCatalog", menuName = "New Master/World Catalog")]
    public sealed class WorldCatalog : ScriptableObject
    {
        [SerializeField] private List<WorldDefinition> worlds = new();

        public IReadOnlyList<WorldDefinition> Worlds => worlds;

        public WorldDefinition Find(int id)
        {
            for (var i = 0; i < worlds.Count; i++)
            {
                if (worlds[i] != null && worlds[i].id == id)
                    return worlds[i];
            }

            return null;
        }

        public static WorldCatalog CreateRuntimeFallback(int worldCount = 365)
        {
            var catalog = CreateInstance<WorldCatalog>();
            catalog.name = "New Master Runtime World Catalog";
            catalog.hideFlags = HideFlags.HideAndDontSave;

            var count = Mathf.Max(1, worldCount);
            for (var id = 1; id <= count; id++)
            {
                var world = CreateInstance<WorldDefinition>();
                world.name = $"World_{id:000}";
                world.hideFlags = HideFlags.HideAndDontSave;
                world.id = id;
                world.worldName = $"World {id:000}";
                world.localizationKey = $"world.{id:000}";
                world.rewardMultiplier = 1f + ((id - 1) * 0.025f);
                world.unlockCost = id == 1 ? 0 : 500L * id * id;
                world.baseSpinReward = 100L + (25L * id);
                world.energyReward = 1 + ((id - 1) / 25);
                world.bossId = id % 10 == 0 ? $"boss.{id:000}" : null;
                world.seasonalTag = id % 20 == 0 ? $"season.{id / 20:00}" : null;
                world.symbols = new List<string> { "Coin", "Crown", "Chest", "Energy", "Hammer" };
                catalog.worlds.Add(world);
            }

            return catalog;
        }

        private void OnDestroy()
        {
            if (hideFlags != HideFlags.HideAndDontSave)
                return;

            for (var i = 0; i < worlds.Count; i++)
            {
                if (worlds[i] != null)
                    Destroy(worlds[i]);
            }
        }

        private void OnValidate()
        {
            var seen = new HashSet<int>();

            foreach (var world in worlds)
            {
                if (world == null)
                    continue;

                if (!seen.Add(world.id))
                    Debug.LogError($"New Master: duplicate world id {world.id}.", this);
            }
        }
    }
}
