using NewMaster.Core;
using NewMaster.Localization;
using NewMaster.Worlds;

namespace NewMaster.Progression
{
    public sealed class ProgressionService
    {
        private readonly EconomyService economy = new();

        public bool TryUnlockNextWorld(GameState state, WorldCatalog catalog)
        {
            if (state == null || catalog == null)
                return false;

            var next = catalog.Find(state.CurrentWorldId + 1);
            if (next == null)
                return false;

            if (!economy.TrySpendCoins(state, next.unlockCost))
                return false;

            state.CurrentWorldId = next.id;
            state.CurrentVillageLevel = 1;
            state.Status.Set(NewMasterTextKeys.NewWorld, next.id, 0, next.localizationKey);
            return true;
        }
    }
}
