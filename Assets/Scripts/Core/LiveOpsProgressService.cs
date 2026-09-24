using System;

namespace NewMaster.Core
{
    public sealed class LiveOpsProgressService
    {
        public bool ActivateEvent(LiveOpsProgressState state, string eventId)
        {
            if (state == null)
                return false;

            var normalized = eventId ?? string.Empty;
            if (state.ActiveEventId == normalized)
                return false;

            state.ActivateEvent(normalized);
            return true;
        }

        public bool AddProgress(
            LiveOpsProgressState state,
            string missionId,
            int amount,
            int target)
        {
            return AddProgress(state, state?.ActiveEventId, missionId, amount, target);
        }

        public bool AddProgress(
            LiveOpsProgressState state,
            string eventId,
            string missionId,
            int amount,
            int target)
        {
            if (state == null || string.IsNullOrWhiteSpace(missionId) || amount <= 0 || target <= 0)
                return false;

            var mission = state.GetOrCreate(eventId, missionId);
            if (mission == null || mission.Claimed)
                return false;

            var before = mission.Progress;
            mission.Progress = (int)Math.Min(
                target,
                (long)mission.Progress + amount);
            return mission.Progress > before;
        }

        public bool TryClaim(
            LiveOpsProgressState state,
            string missionId,
            int target,
            long coinReward,
            int energyReward,
            GameState gameState)
        {
            return TryClaim(
                state,
                state?.ActiveEventId,
                missionId,
                target,
                coinReward,
                energyReward,
                gameState);
        }

        public bool TryClaim(
            LiveOpsProgressState state,
            string eventId,
            string missionId,
            int target,
            long coinReward,
            int energyReward,
            GameState gameState)
        {
            if (state == null || gameState == null ||
                string.IsNullOrWhiteSpace(missionId) || target <= 0)
                return false;

            var mission = state.GetOrCreate(eventId, missionId);
            if (mission == null || mission.Claimed || mission.Progress < target)
                return false;

            var economy = new EconomyService();
            economy.GrantCoins(gameState, Math.Max(0L, coinReward));
            economy.GrantEnergy(gameState, Math.Max(0, energyReward));
            mission.Claimed = true;
            return true;
        }
    }
}
