using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Fighting;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using NativeWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace LGamesDev
{
    /*public class NetworkManager : MonoBehaviour, IObservable<SocketMessage>
    {
        private List<IObserver<SocketMessage>> _observers;

        private const string Url = @"ws://autobattle.hopto.org:35120";
        private WebSocket _ws;

        //private Action _messageCallback;

        public bool isConnected { get; private set; }
        private bool _isError;

        private void Start()
        {
            _observers = new List<IObserver<SocketMessage>>();
        }

        public async void Connect()
        {
            /*_isError = false;
            isConnected = false;
            
            _ws = new WebSocket(Url, new Dictionary<string, string>()
            {
                { "Client", "Unity" },
                { "Authorization", StartManager.Instance.GetAuthentication().token },
            });

            _ws.OnOpen += () =>
            {
                _isError = false;
                //Debug.Log("Connection open!");
                isConnected = true;
            };

            _ws.OnError += (e) =>
            {
                _isError = true;
                Debug.Log("Error! " + e.ToString());
                StartManager.Instance.modalWindow.ShowAsTextPopup(
                    "Something get wrong...",
                    "Error : " + e,
                    "Retry",
                    "Disconnect",
                    StartManager.Instance.LoadMainMenu,
                    StartManager.Instance.Logout,
                    StartManager.Instance.modalWindow.Close
                );
            };

            _ws.OnClose += (e) =>
            {
                //Debug.Log("Connection closed!");
                
                if (!Application.isPlaying) return;
                
                if (!_isError && isConnected)
                {
                    StartManager.Instance.modalWindow.ShowAsTextPopup(
                        "You've got disconnected...",
                        e.ToString(),
                        "Reconnect",
                        "Disconnect",
                        StartManager.Instance.LoadMainMenu,
                        StartManager.Instance.Logout,
                        StartManager.Instance.modalWindow.Close
                    );
                    //Invoke(nameof(Connect), 10.0f);
                }
                isConnected = false;
            };

            _ws.OnMessage += (bytes) =>
            {
                // getting the message as a string
                string response = Encoding.UTF8.GetString(bytes);

                SocketMessage socketMessage = JsonConvert.DeserializeObject<SocketMessage>(response);

                if (socketMessage == null)
                {
                    Debug.Log("socket message is null !"); 
                    return;
                }

                foreach (var observer in _observers) {
                    observer.OnNext(socketMessage);
                }
            };
            
            // waiting for messages
            await _ws.Connect();#1#
        }

        private void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                _ws?.DispatchMessageQueue();
            #endif
        }
        
        public IDisposable Subscribe(IObserver<SocketMessage> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (! _observers.Contains(observer)) {
                //Debug.Log(observer + " subscribed");
                _observers.Add(observer);
                
                // Provide observer with existing data.
                /*foreach (var item in flights)
                    observer.OnNext(item);#1#
            }
            return new UnSubscriber<SocketMessage>(_observers, observer);
        }

        public void SendSocket(SocketMessage socketMessage)
        {
            if (_ws == null || socketMessage == null) return;

            if (_ws.State == WebSocketState.Open)
            {
                /*Debug.Log("sending socket message : " + socketMessage + "\n" +
                          "serialized : " + JsonConvert.SerializeObject(socketMessage));#1#
                
                _ws.SendText(JsonConvert.SerializeObject(socketMessage));
            }
        }

        public async void Disconnect()
        {
            isConnected = false;
            
            if(_ws != null)
                await _ws.Close();
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }
    }*/
}
