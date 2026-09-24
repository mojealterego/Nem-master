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
