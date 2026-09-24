namespace NewMaster.Village
{
    public sealed class VillageCustomizationService
    {
        public bool TrySetCustomization(VillageState state, string customizationId)
        {
            if (state == null || string.IsNullOrWhiteSpace(customizationId)) return false;
            state.CustomizationId = customizationId.Trim();
            return true;
        }
    }
}