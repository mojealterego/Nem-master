using NewMaster.Core;

namespace NewMaster.LiveOps
{
    public sealed class LiveMissionService
    {
        private readonly EconomyService economy = new();

        public bool AddProgress(
            LiveMissionDefinition definition,
            LiveMissionState state,
            int amount)
        {
            if (definition == null || state == null ||
                string.IsNullOrWhiteSpace(definition.missionId) ||
                amount <= 0 ||
                state.Claimed ||
                (state.MissionId != null && state.MissionId != definition.missionId))
                return false;

            if (string.IsNullOrWhiteSpace(state.MissionId))
                state.MissionId = definition.missionId;

            state.Progress = System.Math.Min(
                definition.target,
                state.Progress + amount);
            return true;
        }

        public bool TryClaim(
            LiveMissionDefinition definition,
            LiveMissionState state,
            GameState gameState)
        {
            if (definition == null || state == null || gameState == null ||
                state.MissionId != definition.missionId ||
                state.Claimed ||
                !state.IsComplete(definition.target))
                return false;

            economy.GrantCoins(gameState, definition.coinReward);
            economy.GrantEnergy(gameState, definition.energyReward);
            state.Claimed = true;
            return true;
        }
    }
}
