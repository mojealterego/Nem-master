using System;
using System.Collections.Generic;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class GameState
    {
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public long Coins;
        public int Energy = 10;
        public int CurrentWorldId = 1;
        public int CurrentVillageLevel = 1;
        public int SpinsWon;
        public bool IsSpinning;
        public string StatusMessage = "New Master gotowy.";
        public List<string> Slots = new() { "?", "?", "?" };
    }
}
