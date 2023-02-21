using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LGamesDev.Core;
using LGamesDev.Helper;
using NativeWebSocket;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ChatWindow : MonoBehaviour
    {
        [SerializeField] private MessageUI pfUIMessage;
        [SerializeField] private Transform itemsParent;

        [SerializeField] private TMP_InputField inputField;

        private WebSocket _ws;
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void AddMessageToChannel(Message message)
        {
            MessageUI messageUI = Instantiate(pfUIMessage, itemsParent);
            messageUI.Setup(message.Username, message.Content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageUI.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsParent.transform as RectTransform);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            GameManager.Instance.networkManager.ConnectToChat();
        }

        public void Hide()
        {
            GameManager.Instance.networkManager.ExitChat();
            gameObject.SetActive(false);
        }

        public void SendMessage()
        {
            GameManager.Instance.networkManager.SendMessage(inputField.text);
            inputField.text = "";
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.networkManager.MessageReceived += AddMessageToChannel;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.networkManager.MessageReceived -= AddMessageToChannel;
            }
        }
    }
}
