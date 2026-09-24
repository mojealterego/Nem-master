using System;
using System.Collections.Generic;

namespace NemMaster.Core
{
    [Serializable]
    public sealed class GameState
    {
        public long Coins;
        public int Energy;
        public int CurrentWorldId = 1;
        public bool IsSpinning;
        public string StatusMessage = "Gotowy do gry!";
        public List<string> Slots = new() { "?", "?", "?" };
    }
}
