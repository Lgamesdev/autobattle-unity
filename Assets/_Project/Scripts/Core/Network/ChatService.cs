using System;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Core;
using LGamesDev.Fighting;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Network
{
    public class ChatService : BaseNetworkService
    {
        //Socket Send Actions
        private const string SendMessageAction = "sendMessage";
        
        //Socket Received Actions
        private const string Message = "message";
        private const string MessageList = "messageList";
        
        //public override string channelName => "Chat";
        
        protected override IDisposable Cancellation { get; set; }

        public delegate void MessageReceivedEvent(Message message);
        public static MessageReceivedEvent MessageReceived;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                Subscribe(GameManager.Instance.networkManager);
            }
        }

        protected override void Subscribe(NetworkManager networkManager)
        {
            Cancellation = networkManager.Subscribe(this);
        }

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
            switch (socketMessage.Channel)
            {
                case DefaultChatChannel:
                    //Debug.Log("new socket message for chatService : " + socketMessage);
                    switch (socketMessage.Action)
                    {
                        case MessageList:
                            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(socketMessage.Content);
                            if (messages != null)
                            {
                                foreach (Message m in messages)
                                {
                                    MessageReceived?.Invoke(m);
                                }
                            }
                            break;
                        case Message:
                            MessageReceived.Invoke(new Message()
                            {
                                Username = socketMessage.Username,
                                Content = socketMessage.Content
                            });
                            break;
                    }
                    break;
            }
        }
        
        //Chat
        public new void SendMessage(string message)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = SendMessageAction,
                Channel = DefaultChatChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = message,
            });*/

            /*// Sending plain text to json
            await _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.SendMessage },
                { "channel", SocketChannel.DefaultChatChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", message }
            }));*/
        }
        
        protected override void OnDestroy()
        {
            Cancellation.Dispose();
        }
    }
}