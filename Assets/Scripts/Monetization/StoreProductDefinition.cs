using UnityEngine;

namespace NewMaster.Monetization
{
    [CreateAssetMenu(fileName = "StoreProductDefinition", menuName = "New Master/Monetization Product")]
    public sealed class StoreProductDefinition : ScriptableObject
    {
        public string productId;
        public string localizationKey;
        [Min(0)] public long softCurrencyAmount;
        [Min(0)] public int energyAmount;
    }
}
