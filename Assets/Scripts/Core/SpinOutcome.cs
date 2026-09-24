using System;
using System.Collections.Generic;

namespace NewMaster.Core
{
    public enum SpinOutcomeType
    {
        Nothing,
        Coins,
        Energy,
        VillageProgress,
        Jackpot
    }

    [Serializable]
    public sealed class SpinOutcome
    {
        public IReadOnlyList<string> Slots { get; }
        public SpinOutcomeType Type { get; }
        public long Coins { get; }
        public int Energy { get; }
        public int VillageProgress { get; }

        public SpinOutcome(
            IReadOnlyList<string> slots,
            SpinOutcomeType type,
            long coins = 0,
            int energy = 0,
            int villageProgress = 0)
        {
            Slots = slots;
            Type = type;
            Coins = coins;
            Energy = energy;
            VillageProgress = villageProgress;
        }
    }
}
