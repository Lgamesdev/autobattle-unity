using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using Websocket.Client;

namespace LGamesDev.UI
{
    public class ChatWindow : MonoBehaviour
    {
        private string _defaultChannel = "general";

        [SerializeField] private MessageUI pfUIMessage;
        [SerializeField] private Transform itemsParent;

        [SerializeField] private TMP_InputField inputField;

        private WebSocket _ws;

        private void Awake()
        {
            Hide();
        }

        // Start is called before the first frame update
        private async void Start()
        {
            _ws = new WebSocket("ws://autobattle.hopto.org:35120?access_token=" + GameManager.Instance.GetAuthentication().refresh_token);

            _ws.OnOpen += () =>
            {
                Debug.Log("Connection open!");
                _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    { "action", "subscribe" },
                    { "channel", _defaultChannel },
                    { "user", GameManager.Instance.GetAuthentication().username }
                }));
            };

            _ws.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            _ws.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            _ws.OnMessage += (bytes) =>
            {
                // getting the message as a string
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log("OnMessage! " + message);
                
                Dictionary<string, string> socketMessage =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                AddMessageToChannel(socketMessage?["user"], socketMessage?["message"]);
            };

            // waiting for messages
            await _ws.Connect();
        }

        void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                _ws.DispatchMessageQueue();
            #endif
        }
        
        private void AddMessageToChannel(string author, string content)
        {
            MessageUI messageUI = Instantiate(pfUIMessage, itemsParent);
            messageUI.Setup(author, content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageUI.transform as RectTransform);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SendMessage()
        {
            if(_ws == null)
            {
                return;
            }
            else
            {
                SendWebSocketMessage(inputField.text);
                inputField.text = "";
            }
        }

        async void SendWebSocketMessage(string message)
        {
            if (_ws.State == WebSocketState.Open)
            {
                // Sending plain text to json
                await _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"action", "message" },
                    {"channel", _defaultChannel },
                    {"user", GameManager.Instance.GetAuthentication().username },
                    {"message", message }
                }));
            }
        }

        private async void OnApplicationQuit()
        {
            await _ws.Close();
        }
    }
}
