using System;
using System.IO;
using UnityEngine;

namespace NewMaster.Core
{
    public sealed class LocalSaveService
    {
        private const string FileName = "new_master_save.json";
        private const string VillageFileName = "new_master_village.json";

        [Serializable]
        private sealed class SaveEnvelope
        {
            public int Version;
            public string Payload;
        }

        public void Save<T>(T value, int version)
        {
            var envelope = new SaveEnvelope
            {
                Version = version,
                Payload = JsonUtility.ToJson(value)
            };

            var json = JsonUtility.ToJson(envelope);
            File.WriteAllText(GetPath(fileName), json);
        }

        public void SaveVillage<T>(T value, int version) => Save(value, version, VillageFileName);

        public bool TryLoadVillage<T>(out T value) => TryLoad(out value, VillageFileName);

        private void Save<T>(T value, int version, string fileName)
        {
            value = default;

            var path = GetPath(fileName);
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
            catch (Exception exception)
            {
                Debug.LogWarning($"New Master save could not be loaded: {exception.Message}");
                return false;
            }
        }

        private bool TryLoad<T>(out T value, string fileName)
    }
}
