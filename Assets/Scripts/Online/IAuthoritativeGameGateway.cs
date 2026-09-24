using System;

namespace NewMaster.Online
{
    public interface IAuthoritativeGameGateway
    {
        bool IsConnected { get; }
        void Connect(Action<bool> completed);
        void Disconnect();
    }
}
