using System;
namespace NewMaster.Online
{
    public interface IAuthenticationGateway
    {
        bool IsSignedIn { get; }
        void SignIn(Action<bool> completed);
        void SignOut();
    }
}
