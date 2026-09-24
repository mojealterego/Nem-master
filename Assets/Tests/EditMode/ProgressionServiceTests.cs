using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Progression;
using NewMaster.Worlds;
using UnityEngine;

namespace NewMaster.Tests
{
    public sealed class ProgressionServiceTests
    {
        [Test]
        public void UnlockConsumesCoinsAndMovesToNextWorld()
        {
            var world1 = ScriptableObject.CreateInstance<WorldDefinition>();
            var world2 = ScriptableObject.CreateInstance<WorldDefinition>();
            world1.id = 1;
            world2.id = 2;
            world2.unlockCost = 500;

            var catalog = ScriptableObject.CreateInstance<WorldCatalog>();
            var serialized = new UnityEditor.SerializedObject(catalog);
            var worlds = serialized.FindProperty("worlds");
            worlds.arraySize = 2;
            worlds.GetArrayElementAtIndex(0).objectReferenceValue = world1;
            worlds.GetArrayElementAtIndex(1).objectReferenceValue = world2;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            var state = new GameState { Coins = 500, CurrentWorldId = 1 };
            var service = new ProgressionService();

            Assert.That(service.TryUnlockNextWorld(state, catalog), Is.True);
            Assert.That(state.Coins, Is.EqualTo(0));
            Assert.That(state.CurrentWorldId, Is.EqualTo(2));

            Object.DestroyImmediate(world1);
            Object.DestroyImmediate(world2);
            Object.DestroyImmediate(catalog);
        }
    }
}
