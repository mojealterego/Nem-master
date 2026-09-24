namespace NewMaster.Core
{
    public sealed class BattlePassProgressionService
    {
        public int AddExperience(BattlePassState state, int amount)
        {
            if (state == null || amount <= 0)
                return 0;

            var before = state.Experience;
            state.Experience = (int)System.Math.Min(
                int.MaxValue,
                (long)state.Experience + amount);
            return state.Experience - before;
        }
    }
}
