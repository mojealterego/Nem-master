using System;
using NUnit.Framework;
using NewMaster.Core;

namespace NewMaster.Tests
{
    public sealed class DailyRewardServiceTests
    {
        [Test]
        public void FirstClaimGrantsDayOneReward()
        {
            var state = new GameState();
            var service = new DailyRewardService();
            var now = new DateTimeOffset(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

            Assert.That(service.TryClaim(state, now), Is.True);
            Assert.That(state.Coins, Is.EqualTo(500));
            Assert.That(state.Energy, Is.EqualTo(10));
            Assert.That(state.DailyReward.StreakDay, Is.EqualTo(1));
            Assert.That(state.DailyReward.TotalClaims, Is.EqualTo(1));
        }

        [Test]
        public void SecondClaimOnSameUtcDateIsRejected()
        {
            var state = new GameState();
            var service = new DailyRewardService();
            var now = new DateTimeOffset(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

            Assert.That(service.TryClaim(state, now), Is.True);
            Assert.That(service.TryClaim(state, now.AddHours(6)), Is.False);
            Assert.That(state.DailyReward.TotalClaims, Is.EqualTo(1));
        }

        [Test]
        public void MissingOneDayResetsStreakToDayOne()
        {
            var state = new GameState();
            var service = new DailyRewardService();
            var first = new DateTimeOffset(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

            Assert.That(service.TryClaim(state, first), Is.True);
            Assert.That(service.TryClaim(state, first.AddDays(2)), Is.True);
            Assert.That(state.DailyReward.StreakDay, Is.EqualTo(1));
            Assert.That(state.Coins, Is.EqualTo(1000));
        }

        [Test]
        public void SevenDayCycleRestartsAtDayOne()
        {
            var state = new GameState();
            var service = new DailyRewardService();
            var first = new DateTimeOffset(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

            for (var day = 0; day < 7; day++)
                Assert.That(service.TryClaim(state, first.AddDays(day)), Is.True);

            Assert.That(state.DailyReward.StreakDay, Is.EqualTo(7));
            Assert.That(service.TryClaim(state, first.AddDays(7)), Is.True);
            Assert.That(state.DailyReward.StreakDay, Is.EqualTo(1));
        }

        [Test]
        public void ConsecutiveClaimAdvancesStreak()
        {
            var state = new GameState();
            var service = new DailyRewardService();
            var first = new DateTimeOffset(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

            Assert.That(service.TryClaim(state, first), Is.True);
            Assert.That(service.TryClaim(state, first.AddDays(1)), Is.True);
            Assert.That(state.DailyReward.StreakDay, Is.EqualTo(2));
            Assert.That(state.Coins, Is.EqualTo(1500));
        }
    }
}
