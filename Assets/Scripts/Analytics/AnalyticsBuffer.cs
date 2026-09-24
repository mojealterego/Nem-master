using System;
using System.Collections.Generic;

namespace NewMaster.Analytics
{
    public sealed class AnalyticsBuffer
    {
        private readonly Queue<AnalyticsEvent> events = new();

        public int Count => events.Count;

        public void Track(string name, string value = null, DateTime? utcNow = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var timestamp = utcNow ?? DateTime.UtcNow;
            events.Enqueue(new AnalyticsEvent
            {
                Name = name,
                Value = value,
                TimestampUnixSeconds = new DateTimeOffset(timestamp).ToUnixTimeSeconds()
            });
        }

        public bool TryDequeue(out AnalyticsEvent analyticsEvent)
        {
            if (events.Count == 0)
            {
                analyticsEvent = null;
                return false;
            }

            analyticsEvent = events.Dequeue();
            return true;
        }
    }
}
