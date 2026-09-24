namespace NewMaster.Online
{
    public sealed class ServerCommandValidator
    {
        public bool IsValidCurrencyDelta(long delta) => delta >= 0 && delta <= 1_000_000_000;
        public bool IsValidEnergyDelta(int delta) => delta >= 0 && delta <= 1000;
        public bool IsValidWorldId(int worldId) => worldId >= 1 && worldId <= 365;
        public bool IsValidPlayerId(string playerId) => !string.IsNullOrWhiteSpace(playerId) && playerId.Length <= 64;
    }
}
