using System;

namespace NewMaster.Social
{
    [Serializable]
    public sealed class SocialPersistenceState
    {
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public SocialState Social = new();
        public GuildState Guild = new();

        public void Normalize()
        {
            Version = CurrentVersion;
            Social ??= new SocialState();
            Guild ??= new GuildState();
            Social.Normalize();
            Guild.Normalize();
        }
    }
}
