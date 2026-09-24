using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class GameStatus
    {
        public string Key = "game.ready";
        public long Amount;
        public int SecondaryValue;
        public string Context;

        public void Set(string key, long amount = 0, int secondaryValue = 0, string context = null)
        {
            Key = key;
            Amount = amount;
            SecondaryValue = secondaryValue;
            Context = context;
        }
    }
}
