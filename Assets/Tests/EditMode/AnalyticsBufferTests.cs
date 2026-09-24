using System;
using NUnit.Framework;
using NewMaster.Analytics;

namespace NewMaster.Tests
{
    public sealed class AnalyticsBufferTests
    {
        [Test]
        public void TrackPreservesEventOrder()
        {
            var buffer = new AnalyticsBuffer();
            var time = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            buffer.Track("spin.started", "world.001", time);
            buffer.Track("spin.completed", "coins", time.AddSeconds(1));

            Assert.That(buffer.TryDequeue(out var first), Is.True);
            Assert.That(first.Name, Is.EqualTo("spin.started"));
            Assert.That(buffer.TryDequeue(out var second), Is.True);
            Assert.That(second.Name, Is.EqualTo("spin.completed"));
            Assert.That(buffer.TryDequeue(out _), Is.False);
        }
    }
}
