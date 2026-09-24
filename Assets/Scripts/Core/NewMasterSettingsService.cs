using UnityEngine;

namespace NewMaster.Core
{
    public sealed class NewMasterSettingsService
    {
        private const string Key = "new_master.settings";

        public NewMasterSettings Load()
        {
            var json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
                return new NewMasterSettings();

            try
            {
                var settings = JsonUtility.FromJson<NewMasterSettings>(json);
                return settings ?? new NewMasterSettings();
            }
            catch
            {
                return new NewMasterSettings();
            }
        }

        public void Save(NewMasterSettings settings)
        {
            if (settings == null)
                return;

            settings.MusicVolume = Mathf.Clamp01(settings.MusicVolume);
            settings.SfxVolume = Mathf.Clamp01(settings.SfxVolume);

            PlayerPrefs.SetString(Key, JsonUtility.ToJson(settings));
            PlayerPrefs.Save();
        }
    }
}
