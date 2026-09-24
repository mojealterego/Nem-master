using System;
namespace NewMaster.Monetization
{
    public interface IRewardedAdGateway
    {
        bool IsReady { get; }
        void Show(string placementId, Action<bool> completed);
    }
}
