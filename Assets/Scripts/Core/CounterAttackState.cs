using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class CounterAttackState
    {
        public string TargetPlayerId;
        public long Bounty;
        public bool Available;

        public void Set(string targetPlayerId, long bounty)
        {
            TargetPlayerId = targetPlayerId?.Trim();
            Bounty = Math.Max(0L, bounty);
            Available = !string.IsNullOrWhiteSpace(TargetPlayerId) && Bounty > 0;
        }

        public void Clear()
        {
            TargetPlayerId = null;
            Bounty = 0;
            Available = false;
        }

        public void Normalize()
        {
            TargetPlayerId = TargetPlayerId?.Trim();
            Bounty = Math.Max(0L, Bounty);
            Available = Available && !string.IsNullOrWhiteSpace(TargetPlayerId) && Bounty > 0;
        }
    }
}
