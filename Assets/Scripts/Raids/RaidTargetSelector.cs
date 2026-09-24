using System;
using System.Collections.Generic;

namespace NewMaster.Raids
{
    public sealed class RaidTargetSelector
    {
        public IReadOnlyList<RaidTarget> FilterAvailable(
            IEnumerable<RaidTarget> targets,
            string currentPlayerId)
        {
            var result = new List<RaidTarget>();
            if (targets == null)
                return result;

            foreach (var target in targets)
            {
                if (target == null ||
                    string.IsNullOrWhiteSpace(target.TargetId) ||
                    string.Equals(target.TargetId, currentPlayerId, StringComparison.Ordinal) ||
                    target.AvailableLoot <= 0)
                    continue;

                result.Add(target);
            }

            return result;
        }

        public RaidTarget Select(
            IEnumerable<RaidTarget> targets,
            string currentPlayerId,
            int index)
        {
            var available = FilterAvailable(targets, currentPlayerId);
            if (available.Count == 0 || index < 0 || index >= available.Count)
                return null;

            return available[index];
        }
    }
}
