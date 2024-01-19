using System;
using LGamesDev;
using LGamesDev.Core;

namespace Core.Network
{
    public class RankService : BaseNetworkService
    {
        protected override IDisposable Cancellation { get; set; }
        
        //public override string channelName => "rank";

        private void Start()
        {
            //Subscribe(StartManager.Instance.networkManager);
        }

        /*protected override void Subscribe(NetworkManager networkManager)
        {
            Cancellation = networkManager.Subscribe(this);
        }*/

        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(SocketMessage socketMessage)
        {
            HandleSocket(socketMessage);
        }

        protected override void HandleSocket(SocketMessage socketMessage)
        {
            
        }
        
        protected override void OnDestroy()
        {
            Cancellation.Dispose();
        }
    }
}