using System.Collections.Generic;
using NativeWebSocket;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class ChatWindow : MonoBehaviour
    {
        [SerializeField] private MessageUI pfUIMessage;
        [SerializeField] private Transform itemsParent;

        [SerializeField] private TMP_InputField inputField;
        
        private WebSocket _websocket;

        private void Awake()
        {
            Hide();
        }

        // Start is called before the first frame update
        async void Start()
        {
            _websocket = new WebSocket("ws://autobattle.hopto.org:35120");

            _websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            _websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            _websocket.OnMessage += (bytes) =>
            {
                // getting the message as a string
                string content = System.Text.Encoding.UTF8.GetString(bytes);

                UpdateChatUI("author", content);
            };

            // waiting for messages
            await _websocket.Connect();
        }

        private void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                _websocket.DispatchMessageQueue();
            #endif
        }

        private void UpdateChatUI(string author, string content)
        {
            MessageUI messageUI = Instantiate(pfUIMessage, itemsParent);

            messageUI.Setup(author, content);
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
            if(_websocket == null)
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
            if (_websocket.State == WebSocketState.Open)
            {
                // Sending plain text
                await _websocket.SendText(message);
            }
        }

        private async void OnApplicationQuit()
        {
            await _websocket.Close();
        }
    }
}
