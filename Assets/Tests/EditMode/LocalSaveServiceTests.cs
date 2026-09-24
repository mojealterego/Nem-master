using System.IO;
using NUnit.Framework;
using UnityEngine;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class LocalSaveServiceTests
    {
        private const string SaveFileName = "new_master_save.json";
        private const string BackupSuffix = ".bak";

        [TearDown]
        public void Cleanup()
        {
            Delete(SaveFileName);
            Delete(SaveFileName + BackupSuffix);
            Delete(SaveFileName + ".tmp");
        }

        [Test]
        public void SaveAndLoadRoundTripPreservesState()
        {
            var service = new LocalSaveService();
            var original = new GameState
            {
                Coins = 12345,
                Energy = 7,
                CurrentWorldId = 12,
                CurrentVillageLevel = 4
            };

            service.Save(original, GameState.CurrentVersion);

            Assert.That(service.TryLoad<GameState>(out var loaded), Is.True);
            Assert.That(loaded.Coins, Is.EqualTo(original.Coins));
            Assert.That(loaded.Energy, Is.EqualTo(original.Energy));
            Assert.That(loaded.CurrentWorldId, Is.EqualTo(original.CurrentWorldId));
            Assert.That(loaded.CurrentVillageLevel, Is.EqualTo(original.CurrentVillageLevel));
        }

        [Test]
        public void CorruptPrimarySaveFallsBackToPreviousBackup()
        {
            var service = new LocalSaveService();
            service.Save(new GameState { Coins = 100 }, GameState.CurrentVersion);
            service.Save(new GameState { Coins = 200 }, GameState.CurrentVersion);

            File.WriteAllText(GetPath(SaveFileName), "{corrupt");

            Assert.That(service.TryLoad<GameState>(out var loaded), Is.True);
            Assert.That(loaded.Coins, Is.EqualTo(100));
        }

        private static string GetPath(string fileName) =>
            Path.Combine(Application.persistentDataPath, fileName);

        private static void Delete(string fileName)
        {
            var path = GetPath(fileName);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
