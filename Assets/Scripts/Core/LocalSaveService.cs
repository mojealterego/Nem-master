using System;
using System.IO;
using UnityEngine;

namespace NewMaster.Core
{
    public sealed class LocalSaveService
    {
        private const string FileName = "new_master_save.json";

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
            File.WriteAllText(GetPath(), json);
        }

        public bool TryLoad<T>(out T value)
        {
            value = default;

            var path = GetPath();
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

        private static string GetPath() =>
            Path.Combine(Application.persistentDataPath, FileName);
    }
}
