namespace NewMaster.Core
{
    public static class GameStateMigrations
    {
        public static void Normalize(GameState state)
        {
            if (state == null)
                return;

            if (state.Version < 2)
            {
                if (state.RaidTokens <= 0)
                    state.RaidTokens = 1;

                state.Version = 2;
            }

            state.Energy = System.Math.Max(0, state.Energy);
            state.CurrentWorldId = System.Math.Max(1, state.CurrentWorldId);
            state.CurrentVillageLevel = System.Math.Max(1, state.CurrentVillageLevel);
            state.Slots ??= new System.Collections.Generic.List<string> { "?", "?", "?" };
        }
    }
}
