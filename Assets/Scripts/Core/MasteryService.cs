using System;

namespace NewMaster.Core
{
    public sealed class MasteryService
    {
        public const int MaxRank = 1000;

        public long AddExperience(MasteryState state, long amount)
        {
            if (state == null || amount <= 0)
                return 0;

            state.Normalize();
            var before = state.Experience;
            state.Experience = SaturatingAdd(state.Experience, amount);
            UpdateRank(state);
            return state.Experience - before;
        }

        public int GetRank(long experience)
        {
            experience = Math.Max(0L, experience);
            var rank = 0;
            var remaining = experience;

            while (rank < MaxRank)
            {
                var required = GetExperienceForNextRank(rank);
                if (remaining < required)
                    break;

                remaining -= required;
                rank++;
            }

            return rank;
        }

        public long GetExperienceForNextRank(int rank)
        {
            rank = Math.Max(0, Math.Min(MaxRank - 1, rank));
            return 500L + (long)rank * 250L + (long)rank * rank * 25L;
        }

        public int GetMilestonesReached(long previousExperience, long currentExperience)
        {
            previousExperience = Math.Max(0L, previousExperience);
            currentExperience = Math.Max(previousExperience, currentExperience);

            var before = GetRank(previousExperience);
            var after = GetRank(currentExperience);
            return Math.Max(0, after - before);
        }

        private void UpdateRank(MasteryState state)
        {
            state.Rank = Math.Max(state.Rank, GetRank(state.Experience));
            if (state.Rank > MaxRank)
                state.Rank = MaxRank;
        }

        private static long SaturatingAdd(long current, long amount)
        {
            if (amount <= 0)
                return current;

            if (current > long.MaxValue - amount)
                return long.MaxValue;

            return current + amount;
        }
    }
}
