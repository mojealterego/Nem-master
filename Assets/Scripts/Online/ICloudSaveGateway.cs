using System;

namespace NewMaster.Online
{
    public interface ICloudSaveGateway
    {
        bool IsAvailable { get; }
        void Load(string playerId, Action<string> completed);
        void Save(string playerId, string payload, Action<bool> completed);
        void ResolveConflict(string playerId, string localPayload, string remotePayload, Action<string> completed);
    }
}
