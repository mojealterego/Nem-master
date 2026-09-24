using System;
using UnityEngine;

namespace NewMaster.LiveOps
{
    [CreateAssetMenu(fileName = "LiveEventDefinition", menuName = "New Master/LiveOps Event")]
    public sealed class LiveEventDefinition : ScriptableObject
    {
        public string eventId = "event.example";
        public string localizationKey = "event.example";
        public DateTime StartUtc => startUtc;
        public DateTime EndUtc => endUtc;
        [SerializeField] private string startUtcIso = "2026-01-01T00:00:00Z";
        [SerializeField] private string endUtcIso = "2026-01-08T00:00:00Z";

        private DateTime startUtc => ParseUtc(startUtcIso);
        private DateTime endUtc => ParseUtc(endUtcIso);

        public bool IsActive(DateTime utcNow) => utcNow >= StartUtc && utcNow < EndUtc;

        private static DateTime ParseUtc(string value)
        {
            return DateTime.TryParse(
                value,
                null,
                System.Globalization.DateTimeStyles.AssumeUniversal |
                System.Globalization.DateTimeStyles.AdjustToUniversal,
                out var result)
                ? result
                : DateTime.MinValue;
        }
    }
}
