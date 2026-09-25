using System;
using System.IO;
using UnityEngine;

namespace NewMaster.Core
{
    public sealed class LocalSaveService
    {
        private const string FileName = "new_master_save.json";
        private const string VillageFileName = "new_master_village.json";
        private const string CollectionsFileName = "new_master_collections.json";
        private const string SocialFileName = "new_master_social.json";
        private const string BackupSuffix = ".bak";

        [Serializable]
        private sealed class SaveEnvelope
        {
            public int Version;
            public string Payload;
        }

        public void Save<T>(T value, int version) =>
            Save(value, version, FileName);

        public void SaveVillage<T>(T value, int version) =>
            Save(value, version, VillageFileName);

        public void SaveCollections<T>(T value, int version) =>
            Save(value, version, CollectionsFileName);

        public bool TryLoad<T>(out T value) =>
            TryLoad(out value, FileName);

        public bool TryLoadVillage<T>(out T value) =>
            TryLoad(out value, VillageFileName);

        public bool TryLoadCollections<T>(out T value) =>
            TryLoad(out value, CollectionsFileName);

        public void SaveSocial<T>(T value, int version) =>
            Save(value, version, SocialFileName);

        public bool TryLoadSocial<T>(out T value) =>
            TryLoad(out value, SocialFileName);

        private static void Save<T>(T value, int version, string fileName)
        {
            var path = GetPath(fileName);
            var temporaryPath = path + ".tmp";
            var backupPath = path + BackupSuffix;

            var envelope = new SaveEnvelope
            {
                Version = version,
                Payload = JsonUtility.ToJson(value)
            };

            try
            {
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(envelope));

                if (File.Exists(path))
                {
                    TryCreateBackup(path, backupPath);

                    try
                    {
                        File.Replace(temporaryPath, path, null);
                    }
                    catch (PlatformNotSupportedException)
                    {
                        File.Delete(path);
                        File.Move(temporaryPath, path);
                    }
                }
                else
                {
                    File.Move(temporaryPath, path);
                }
            }
            catch (Exception exception)
            {
                TryDelete(temporaryPath);
                Debug.LogWarning($"New Master save could not be written: {exception.Message}");
            }
        }

        private static bool TryLoad<T>(out T value, string fileName)
        {
            value = default;

            var path = GetPath(fileName);
            if (TryRead(path, out value))
                return true;

            var backupPath = path + BackupSuffix;
            if (TryRead(backupPath, out value))
            {
                Debug.LogWarning($"New Master recovered save data from backup: {backupPath}");
                return true;
            }

            return false;
        }

        private static bool TryRead<T>(string path, out T value)
        {
            value = default;

            if (!File.Exists(path))
                return false;

            try
            {
                var envelope = JsonUtility.FromJson<SaveEnvelope>(File.ReadAllText(path));
                if (envelope == null || string.IsNullOrEmpty(envelope.Payload))
                    return false;

                value = JsonUtility.FromJson<T>(envelope.Payload);
                return value != null;
            }
            catch
            {
                return false;
            }
        }

        private static void TryCreateBackup(string sourcePath, string backupPath)
        {
            try
            {
                File.Copy(sourcePath, backupPath, true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"New Master save backup could not be created: {exception.Message}");
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Best-effort cleanup only.
            }
        }

        private static string GetPath(string fileName) =>
            Path.Combine(Application.persistentDataPath, fileName);
    }
}
