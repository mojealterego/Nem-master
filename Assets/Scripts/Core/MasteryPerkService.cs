namespace NewMaster.Core
{
    public sealed class MasteryPerkService
    {
        public const int MaxCoinBonusPercent = 20;
        public const int CoinBonusRankStep = 25;
        public const int EnergyBonusRankThreshold = 100;

        public int GetCoinBonusPercent(int rank)
        {
            if (rank <= 0)
                return 0;

            var percent = rank / CoinBonusRankStep;
            return percent > MaxCoinBonusPercent ? MaxCoinBonusPercent : percent;
        }

        public int GetEnergyRewardBonus(int rank)
        {
            return rank >= EnergyBonusRankThreshold ? 1 : 0;
        }

        public int GetPerkTier(int rank)
        {
            return rank <= 0 ? 0 : rank / CoinBonusRankStep;
        }

        public long ApplyCoinBonus(long baseCoins, int rank)
        {
            if (baseCoins <= 0)
                return 0;

            var percent = GetCoinBonusPercent(rank);
            if (percent <= 0)
                return baseCoins;

            var bonus = baseCoins / 100L * percent;
            var remainder = baseCoins % 100L;
            bonus = SafeAdd(bonus, remainder * percent / 100L);
            return SafeAdd(baseCoins, bonus);
        }

        private static long SafeAdd(long left, long right)
        {
            if (right <= 0)
                return left;

            return left > long.MaxValue - right
                ? long.MaxValue
                : left + right;
        }
    }
}
