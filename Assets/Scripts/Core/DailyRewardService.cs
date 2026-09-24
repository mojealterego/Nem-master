using System;

namespace NewMaster.Core
{
    public readonly struct DailyRewardPreview
    {
        public DailyRewardPreview(bool available, int day, long coins, int energy, int streak)
        {
            IsAvailable = available;
            Day = day;
            Coins = coins;
            Energy = energy;
            Streak = streak;
        }

        public bool IsAvailable { get; }
        public int Day { get; }
        public long Coins { get; }
        public int Energy { get; }
        public int Streak { get; }
    }

    public sealed class DailyRewardService
    {
        private static readonly long[] CoinRewards =
        {
            500L, 1000L, 1500L, 2500L, 4000L, 6500L, 10000L
        };

        private readonly EconomyService economy = new();

        public DailyRewardPreview Preview(GameState state, DateTimeOffset nowUtc)
        {
            if (state == null)
                return new DailyRewardPreview(false, 0, 0, 0, 0);

            var rewardState = state.DailyReward ??= new DailyRewardState();
            rewardState.Normalize();

            var today = nowUtc.UtcDateTime.Date;
            if (TryParseDate(rewardState.LastClaimDateUtc, out var lastClaim))
            {
                var daysSinceClaim = (today - lastClaim.Date).TotalDays;
                if (daysSinceClaim < 1)
                    return new DailyRewardPreview(false, rewardState.StreakDay, 0, 0, rewardState.StreakDay);

                var nextDay = daysSinceClaim > 1 ? 1 : Math.Min(7, rewardState.StreakDay + 1);
                return CreatePreview(nextDay, rewardState.StreakDay);
            }

            return CreatePreview(1, 0);
        }

        public bool TryClaim(GameState state, DateTimeOffset nowUtc)
        {
            if (state == null)
                return false;

            var preview = Preview(state, nowUtc);
            if (!preview.IsAvailable)
                return false;

            var rewardState = state.DailyReward ??= new DailyRewardState();
            var grantedCoins = economy.GrantCoins(state, preview.Coins);
            var grantedEnergy = economy.GrantEnergy(state, preview.Energy);

            if (grantedCoins <= 0 && grantedEnergy <= 0)
                return false;

            rewardState.StreakDay = preview.Day;
            rewardState.TotalClaims = rewardState.TotalClaims < int.MaxValue
                ? rewardState.TotalClaims + 1
                : int.MaxValue;
            rewardState.LastClaimDateUtc = nowUtc.UtcDateTime.Date.ToString("yyyy-MM-dd");

            return true;
        }

        private static DailyRewardPreview CreatePreview(int day, int currentStreak)
        {
            var safeDay = Math.Max(1, Math.Min(7, day));
            var energy = safeDay % 3 == 0 ? 1 : 0;
            return new DailyRewardPreview(
                true,
                safeDay,
                CoinRewards[safeDay - 1],
                energy,
                safeDay);
        }

        private static bool TryParseDate(string value, out DateTime date)
        {
            return DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date);
        }
    }
}
