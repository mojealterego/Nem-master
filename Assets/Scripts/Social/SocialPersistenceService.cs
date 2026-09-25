using NewMaster.Core;

namespace NewMaster.Social
{
    public sealed class SocialPersistenceService
    {
        private const int Version = SocialPersistenceState.CurrentVersion;
        private readonly LocalSaveService saveService;

        public SocialPersistenceService(LocalSaveService saveService = null)
        {
            this.saveService = saveService ?? new LocalSaveService();
        }

        public void Save(SocialPersistenceState state)
        {
            if (state == null)
                return;

            state.Normalize();
            saveService.SaveSocial(state, Version);
        }

        public bool TryLoad(out SocialPersistenceState state)
        {
            if (!saveService.TryLoadSocial(out state) || state == null)
            {
                state = new SocialPersistenceState();
                state.Normalize();
                return false;
            }

            state.Normalize();
            return true;
        }
    }
}
