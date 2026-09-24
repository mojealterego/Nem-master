using NewMaster.Core;
using NewMaster.Localization;

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
            {
                gameState.Status.Set(NewMasterTextKeys.RaidNoToken);
                return new RaidResult { Type = RaidResultType.Blocked };
            }

            var result = raidService.Resolve(target, true);

            if (result.Type != RaidResultType.Blocked || result.ShieldsConsumed > 0)
                gameState.RaidTokens--;

            if (result.Loot > 0)
            {
                gameState.Coins += result.Loot;
                gameState.Status.Set(NewMasterTextKeys.RaidLoot, result.Loot);
            }
            else if (result.ShieldsConsumed > 0)
            {
                gameState.Status.Set(NewMasterTextKeys.RaidShieldBlocked);
            }
            else
            {
                gameState.Status.Set(NewMasterTextKeys.RaidEmptyTarget);
            }

            return result;
        }
    }
}
