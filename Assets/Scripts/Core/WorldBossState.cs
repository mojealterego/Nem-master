using System;

namespace NewMaster.Core
{
    [Serializable]
    public sealed class WorldBossState
    {
        public string BossId;
        public long MaxHealth;
        public long Health;
        public long PersonalContribution;
        public long GuildContribution;
        public bool Defeated;
        public bool RewardClaimed;

        public void Initialize(string bossId, long maxHealth)
        {
            BossId = bossId?.Trim();
            MaxHealth = Math.Max(1L, maxHealth);
            Health = MaxHealth;
            PersonalContribution = 0;
            GuildContribution = 0;
            Defeated = false;
            RewardClaimed = false;
        }

        public void Normalize()
        {
            BossId = BossId?.Trim();
            MaxHealth = Math.Max(1L, MaxHealth);
            Health = Math.Max(0L, Math.Min(MaxHealth, Health));
            PersonalContribution = Math.Max(0L, PersonalContribution);
            GuildContribution = Math.Max(0L, GuildContribution);
            if (Health == 0)
                Defeated = true;
            if (!Defeated)
                RewardClaimed = false;
        }
    }
}
