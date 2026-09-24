using System;

namespace NewMaster.Online
{
    public interface IMatchmakingGateway
    {
        bool IsConnected { get; }
        void FindMatch(string mode, Action<bool> completed);
        void Cancel();
        void LeaveMatch();
    }
}
