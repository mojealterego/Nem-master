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

            economy.GrantCoins(state, product.softCurrencyAmount);
            economy.GrantEnergy(state, product.energyAmount);
            return true;
        }
    }
}
