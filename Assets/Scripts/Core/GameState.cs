using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class GameState
    {
        public const int CurrentVersion = 5;

        public int Version = CurrentVersion;
        public long Coins;
        public int Energy = 10;
        public int CurrentWorldId = 1;
        public int CurrentVillageLevel = 1;
        public int SpinsWon;
        public int RaidTokens = 1;
        public bool IsSpinning;
        public GameStatus Status = new();
        public BattlePassState BattlePass = new();
        public GameSessionState Session = new();
        public System.Collections.Generic.List<string> Slots = new() { "?", "?", "?" };

        [Obsolete("Use Status instead.")]
        public string StatusMessage;
    }
}
