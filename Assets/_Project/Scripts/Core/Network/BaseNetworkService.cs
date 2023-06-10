using System;
using LGamesDev.Core;
using UnityEngine;

namespace LGamesDev
{
    public abstract class BaseNetworkService : MonoBehaviour, IObserver<SocketMessage>
    {
        //Channel names
        //public abstract string channelName { get; }
        protected const string DefaultChannel = "general";
        protected const string DefaultChatChannel = "chat_general";
        protected const string ShopChannel = "shop";
        protected const string RankChannel = "rank";
        protected const string FightChannelSuffix = "fight_";

        protected abstract IDisposable Cancellation { get; set; }

        protected abstract void Subscribe(NetworkManager networkManager);

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(SocketMessage socketMessage);
        
        protected abstract void HandleSocket(SocketMessage socketMessage);

        protected abstract void OnDestroy();
    }
}