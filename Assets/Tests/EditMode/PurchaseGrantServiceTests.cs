using NUnit.Framework;
using NewMaster.Core;
using NewMaster.Monetization;
using UnityEngine;
namespace NewMaster.Tests
{
    public sealed class PurchaseGrantServiceTests
    {
        [Test]
        public void PurchaseGrantAddsConfiguredRewards()
        {
            var product = ScriptableObject.CreateInstance<StoreProductDefinition>();
            try
            {
                product.productId = "coins.001";
                product.softCurrencyAmount = 500;
                product.energyAmount = 3;
                var state = new GameState();
                Assert.That(new PurchaseGrantService().Grant(product, state), Is.True);
                Assert.That(state.Coins, Is.EqualTo(500));
                Assert.That(state.Energy, Is.EqualTo(13));
            }
            finally { Object.DestroyImmediate(product); }
        }
    }
}
