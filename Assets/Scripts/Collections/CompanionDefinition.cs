using UnityEngine;
namespace NewMaster.Collections
{
    public enum CompanionType { Pet, Artifact }
    [CreateAssetMenu(fileName = "CompanionDefinition", menuName = "New Master/Collections/Companion")]
    public sealed class CompanionDefinition : ScriptableObject
    {
        public string Id = "companion.example";
        public string LocalizationKey = "companion.example";
        public CompanionType Type = CompanionType.Pet;
        [Min(0f)] public float RewardMultiplier = 1f;
        [Min(0)] public int EnergyBonus;
    }
}
