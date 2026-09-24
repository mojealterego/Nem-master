namespace NewMaster.Core
{
    public static class GameStateMigrations
    {
        public static void Normalize(GameState state)
        {
            if (state == null)
                return;

            if (state.Status == null)
                state.Status = new GameStatus();

            if (state.Version < 2)
            {
                if (state.RaidTokens <= 0)
                    state.RaidTokens = 1;

                state.Version = 2;
            }

            if (state.Version < 3)
            {
                MigrateLegacyStatus(state);
                state.Version = 3;
            }

            state.Energy = System.Math.Max(0, state.Energy);
            state.CurrentWorldId = System.Math.Max(1, state.CurrentWorldId);
            state.CurrentVillageLevel = System.Math.Max(1, state.CurrentVillageLevel);
            state.Slots ??= new System.Collections.Generic.List<string> { "?", "?", "?" };

            if (string.IsNullOrWhiteSpace(state.Status.Key))
                state.Status.Set(NewMaster.Localization.NewMasterTextKeys.GameReady);
        }

        private static void MigrateLegacyStatus(GameState state)
        {
            var legacy = state.StatusMessage;
            if (string.IsNullOrWhiteSpace(legacy))
                return;

            if (legacy == "New Master gotowy.")
                state.Status.Set(NewMaster.Localization.NewMasterTextKeys.GameReady);
            else if (legacy == "Postęp wioski +1")
                state.Status.Set(NewMaster.Localization.NewMasterTextKeys.VillageProgress);
            else if (legacy == "Brak nagrody. Następny obrót może zmienić wszystko.")
                state.Status.Set(NewMaster.Localization.NewMasterTextKeys.NoReward);
            else
                state.Status.Set(NewMaster.Localization.NewMasterTextKeys.GameReady);

            state.StatusMessage = null;
        }
    }
}
