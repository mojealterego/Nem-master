using UnityEngine;

namespace NewMaster.Core
{
    public sealed class NewMasterHapticsService
    {
        private readonly NewMasterSettingsService settingsService;

        public NewMasterHapticsService(NewMasterSettingsService settingsService = null)
        {
            this.settingsService = settingsService ?? new NewMasterSettingsService();
        }

        public void Pulse()
        {
            var settings = settingsService.Load();
            if (settings.HapticsEnabled)
                Handheld.Vibrate();
        }
    }
}
