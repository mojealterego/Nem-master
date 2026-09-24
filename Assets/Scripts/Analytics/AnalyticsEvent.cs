using System;

namespace NewMaster.Analytics
{
    [Serializable]
    public sealed class AnalyticsEvent
    {
        public string Name;
        public string Value;
        public long TimestampUnixSeconds;
    }
}
