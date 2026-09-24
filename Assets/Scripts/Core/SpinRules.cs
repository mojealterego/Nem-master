using System;
using System.Collections.Generic;

namespace NewMaster.Core
{
    public static class SpinRules
    {
        public static SpinOutcome Resolve(
            IReadOnlyList<string> slots,
            long baseReward,
            int energyReward = 0)
        {
            if (slots == null || slots.Count != 3)
                throw new ArgumentException("New Master spin requires exactly three slots.", nameof(slots));

            if (slots[0] == slots[1] && slots[1] == slots[2])
            {
                return new SpinOutcome(
                    slots,
                    SpinOutcomeType.Jackpot,
                    coins: Math.Max(1000L, baseReward * 5L));
            }

            if (slots[0] == "Coin" && slots[1] == "Coin")
            {
                return new SpinOutcome(
                    slots,
                    SpinOutcomeType.Coins,
                    coins: Math.Max(250L, baseReward));
            }

            if (slots[0] == "Energy" && slots[1] == "Energy")
            {
                return new SpinOutcome(
                    slots,
                    SpinOutcomeType.Energy,
                    energy: Math.Max(1, energyReward));
            }

            if (slots[0] == "Hammer" && slots[1] == "Hammer")
            {
                return new SpinOutcome(
                    slots,
                    SpinOutcomeType.VillageProgress,
                    villageProgress: 1);
            }

            return new SpinOutcome(slots, SpinOutcomeType.Nothing);
        }
    }
}
