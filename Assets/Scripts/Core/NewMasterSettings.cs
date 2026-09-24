using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class NewMasterSettings
    {
        public string LanguageCode = "en";
        public bool HapticsEnabled = true;
        public bool NotificationsEnabled = true;
        public float MusicVolume = 1f;
        public float SfxVolume = 1f;
    }
}
