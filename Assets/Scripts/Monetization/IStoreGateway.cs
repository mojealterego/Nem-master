using System;

namespace NewMaster.Monetization
{
    public interface IStoreGateway
    {
        void Initialize(Action<bool> completed);
        void Purchase(string productId, Action<bool> completed);
        bool IsInitialized { get; }
    }
}
