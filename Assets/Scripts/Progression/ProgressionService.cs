using NewMaster.Core;
using NewMaster.Worlds;

namespace NewMaster.Progression
{
    public sealed class ProgressionService
    {
        public bool TryUnlockNextWorld(GameState state, WorldCatalog catalog)
        {
            if (state == null || catalog == null)
                return false;

            var next = catalog.Find(state.CurrentWorldId + 1);
            if (next == null || state.Coins < next.unlockCost)
                return false;

            state.Coins -= next.unlockCost;
            state.CurrentWorldId = next.id;
            state.CurrentVillageLevel = 1;
            state.StatusMessage = $"Nowy świat: {next.worldName}";
            return true;
        }
    }
}
