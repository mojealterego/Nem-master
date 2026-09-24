using NewMaster.Core;

namespace NewMaster.Raids
{
    public sealed class RaidGameService
    {
        private readonly RaidService raidService = new();

        public RaidResult TryRaid(GameState gameState, RaidTarget target)
        {
            if (gameState == null)
                return new RaidResult { Type = RaidResultType.Blocked };

            if (gameState.RaidTokens <= 0)
                return new RaidResult { Type = RaidResultType.Blocked };

            var result = raidService.Resolve(target, true);

            if (result.Type != RaidResultType.Blocked || result.ShieldsConsumed > 0)
                gameState.RaidTokens--;

            if (result.Loot > 0)
            {
                gameState.Coins += result.Loot;
                gameState.StatusMessage = $"+{result.Loot:N0} łupu";
            }
            else if (result.ShieldsConsumed > 0)
            {
                gameState.StatusMessage = "Tarcza zablokowała rajd.";
            }

            return result;
        }
    }
}
