using System;

namespace NewMaster.Online
{
    public sealed class ServerCommandValidator
    {
        public bool IsValidCurrencyDelta(long delta) =>
            delta >= 0 && delta <= 1_000_000_000;

        public bool IsValidEnergyDelta(int delta) =>
            delta >= 0 && delta <= 1000;

        public bool IsValidWorldId(int worldId) =>
            worldId >= 1 && worldId <= 365;

        public bool IsValidPlayerId(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId) || playerId.Length > 64)
                return false;

            for (var i = 0; i < playerId.Length; i++)
            {
                var c = playerId[i];
                if (char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '.')
                    continue;

                return false;
            }

            return true;
        }

        public bool IsValidMode(string mode) =>
            !string.IsNullOrWhiteSpace(mode) && mode.Length <= 32;

        public bool IsValidProductId(string productId) =>
            !string.IsNullOrWhiteSpace(productId) && productId.Length <= 128;

        public bool IsValidEventId(string eventId) =>
            !string.IsNullOrWhiteSpace(eventId) && eventId.Length <= 128;

        public bool IsValidTimestamp(DateTime utcTimestamp) =>
            utcTimestamp.Kind == DateTimeKind.Utc;
    }
}
