using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.Raids
{
    public sealed class RaidGameService
    {
        private readonly EconomyService economy = new();
        private readonly RaidEngineService raidEngine = new();

        public RaidResult TryRaid(GameState gameState, RaidTarget target)
        {
            var engineResult = TryRaidDetailed(gameState, target);
            return new RaidResult
            {
                Type = engineResult.Type,
                Loot = engineResult.Loot,
                ShieldsConsumed = engineResult.ShieldsConsumed
            };
        }

        public RaidEngineResult TryRaidDetailed(GameState gameState, RaidTarget target)
        {
            if (gameState == null)
                return new RaidEngineResult { Type = RaidResultType.Blocked };

            if (gameState.RaidTokens <= 0)
            {
                gameState.Status.Set(NewMasterTextKeys.RaidNoToken);
                return new RaidEngineResult { Type = RaidResultType.Blocked };
            }

            var result = raidEngine.Resolve(target, true);

            if (result.Type != RaidResultType.Blocked || result.ShieldsConsumed > 0)
            {
                gameState.RaidTokens--;

                if (gameState.Session != null && gameState.Session.RaidsThisSession < int.MaxValue)
                    gameState.Session.RaidsThisSession++;
            }

            if (result.Loot > 0)
            {
                var granted = economy.GrantCoins(gameState, result.Loot);
                gameState.Status.Set(NewMasterTextKeys.RaidLoot, granted);
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
