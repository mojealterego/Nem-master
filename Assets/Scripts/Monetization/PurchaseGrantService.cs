using NewMaster.Core;

namespace NewMaster.Monetization
{
    public sealed class PurchaseGrantService
    {
        private readonly EconomyService economy = new();

        public bool Grant(StoreProductDefinition product, GameState state)
        {
            if (product == null || state == null || string.IsNullOrWhiteSpace(product.productId))
                return false;

            var grantedCoins = economy.GrantCoins(state, product.softCurrencyAmount);
            var grantedEnergy = economy.GrantEnergy(state, product.energyAmount);

            return grantedCoins > 0 || grantedEnergy > 0;
        }
    }
}
