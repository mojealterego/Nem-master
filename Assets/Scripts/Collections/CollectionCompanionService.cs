using System;
using System.Collections.Generic;

namespace NewMaster.Collections
{
    public sealed class CollectionCompanionService
    {
        public float GetRewardMultiplier(
            IEnumerable<CompanionDefinition> definitions,
            CollectionState state)
        {
            var multiplier = 1f;
            if (definitions == null || state == null)
                return multiplier;

            foreach (var definition in definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id))
                    continue;

                var owned = definition.Type == CompanionType.Pet
                    ? state.OwnsPet(definition.Id)
                    : state.OwnsArtifact(definition.Id);

                if (owned)
                    multiplier = Math.Max(multiplier, Math.Max(1f, definition.RewardMultiplier));
            }

            return multiplier;
        }

        public int GetEnergyBonus(
            IEnumerable<CompanionDefinition> definitions,
            CollectionState state)
        {
            var bonus = 0;
            if (definitions == null || state == null)
                return bonus;

            foreach (var definition in definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id))
                    continue;

                var owned = definition.Type == CompanionType.Pet
                    ? state.OwnsPet(definition.Id)
                    : state.OwnsArtifact(definition.Id);

                if (owned)
                    bonus = Math.Max(bonus, Math.Max(0, definition.EnergyBonus));
            }

            return bonus;
        }
    }
}
