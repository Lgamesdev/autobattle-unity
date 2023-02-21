using System;
using System.Collections.Generic;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Fighting;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;

namespace LGamesDev
{
    public class NetworkManager : MonoBehaviour
    {
        private const string Url = @"ws://autobattle.hopto.org:35120";
        private WebSocket _ws;
        
        public delegate void MessageReceivedEvent(Message message);
        public MessageReceivedEvent MessageReceived;

        //private Action _messageCallback;
        private Action _onAttackCallback;

        public bool isConnected { get; private set; }
        private bool _isError;

        public async void Connect()
        {
            _isError = false;
            isConnected = false;
            
            _ws = new WebSocket(Url, new Dictionary<string, string>()
            {
                { "Client", "Unity" },
                { "Authorization", GameManager.Instance.GetAuthentication().token },
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
                //Debug.Log("Error! " + e);
                GameManager.Instance.modalWindow.ShowAsTextPopup(
                    "Something get wrong...",
                    "Error : " + e,
                    "Retry",
                    "Disconnect",
                    GameManager.Instance.LoadMainMenu,
                    GameManager.Instance.Logout
                );
            };

            _ws.OnClose += (e) =>
            {
                //Debug.Log("Connection closed!");
                isConnected = false;
                if (!Application.isPlaying) return;
                
                if (!_isError)
                {
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Something get wrong...",
                        e.ToString(),
                        "Reconnect",
                        "Disconnect",
                        GameManager.Instance.LoadMainMenu,
                        GameManager.Instance.Logout
                    );
                    //Invoke(nameof(Connect), 10.0f);
                }
            };

            _ws.OnMessage += (bytes) =>
            {
                // getting the message as a string
                string response = System.Text.Encoding.UTF8.GetString(bytes);
                //Debug.Log("OnMessage! " + response);
                
                SocketMessage socketMessage = JsonConvert.DeserializeObject<SocketMessage>(response);

                if (socketMessage == null)
                {
                    Debug.Log("socket action is null !"); 
                    return;
                }
                
                HandleSocketChannel(socketMessage);
            };
            
            // waiting for messages
            await _ws.Connect();
        }

        private void HandleSocketChannel(SocketMessage socketMessage)
        {
            switch (socketMessage.Channel)
            {
                case SocketChannel.DefaultChannel:
                    switch (socketMessage.Action)
                    {
                        case SocketAction.Equip:
                            CharacterManager.Instance.equipmentManager.Equip(int.Parse(socketMessage.Content));
                            break;
                        case SocketAction.UnEquip:
                            CharacterManager.Instance.equipmentManager.UnEquip((int)Enum.Parse<EquipmentSlot>(socketMessage.Content));
                            break;
                        case SocketAction.ShopList:
                            JsonSerializerSettings settings = new JsonSerializerSettings();
                            settings.Converters.Add(new ItemConverter());
                            List<Item> shopItems = JsonConvert.DeserializeObject<List<Item>>(socketMessage.Content, settings);
                            ShopUI.Instance.ShopItems = shopItems;
                            break;
                        case SocketAction.RankList:
                            List<Character> characterList = JsonConvert.DeserializeObject<List<Character>>(socketMessage.Content);
                            RankingUI.Instance.CharacterList = characterList;
                            break;
                        case SocketAction.Error:
                            GameManager.Instance.modalWindow.ShowAsTextPopup(
                                "Error",
                                socketMessage.Content,
                                "Ok",
                                "Disconnect",
                                GameManager.Instance.modalWindow.Close,
                                GameManager.Instance.Logout
                            );
                            break;
                        
                    }
                    break;
                case SocketChannel.DefaultChatChannel:
                    switch (socketMessage.Action)
                    {
                        case SocketAction.MessageList:
                            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(socketMessage.Content);
                            if (messages != null)
                            {
                                foreach (Message m in messages)
                                {
                                    MessageReceived.Invoke(m);
                                }
                            }
                            break;
                        case SocketAction.Message:
                            MessageReceived.Invoke(new Message()
                            {
                                Username = socketMessage.Username,
                                Content = socketMessage.Content
                            });
                            break;
                    }
                    break;
                case var value when string.Equals(value, string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username)):
                    switch (socketMessage.Action)
                    {
                        case SocketAction.StartFight:
                            Fight fight = JsonConvert.DeserializeObject<Fight>(socketMessage.Content);
                            GameManager.Instance.LoadFight(fight);
                            break;
                        case SocketAction.Attack:
                            List<FightAction> fightActions = JsonConvert.DeserializeObject<List<FightAction>>(socketMessage.Content);
                            FightManager.Instance.Attack(fightActions, _onAttackCallback);
                            break;
                        case SocketAction.EndFight:
                            Reward reward = JsonConvert.DeserializeObject<Reward>(socketMessage.Content);
                            FightManager.Instance.Fight.Reward = reward;
                            break;
                    }
                    break;
            }
        }

        private void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                _ws?.DispatchMessageQueue();
            #endif
        }
        
        //Body
        public void GetBody()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.ShopList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }

        //Chat
        public void ConnectToChat()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.Subscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));
        }
        
        public new async void SendMessage(string message)
        {
            if (_ws == null) return;
            
            if (_ws.State == WebSocketState.Open)
            {
                // Sending plain text to json
                await _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    { "action", "message" },
                    { "channel", SocketChannel.DefaultChatChannel },
                    { "username", GameManager.Instance.GetAuthentication().username },
                    { "content", message }
                }));
            }
        }

        public void ExitChat()
        {
            _ws?.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.Unsubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));
        }
        
        //Gear
        public void Equip(CharacterEquipment newEquipment)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.Equip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", newEquipment.id.ToString() }
            }));
        }
        
        public void UnEquip(CharacterEquipment characterEquipment)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.UnEquip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", characterEquipment.id.ToString() }
            }));
        }
        
        //Shop
        public void GetItemsList()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.ShopList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }
        
        //Rank
        public void GetRankList()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.RankList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }
        
        //Fight
        public void SearchFight()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.Subscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username) }
            }));
        }

        public void Attack(Action onAttackCallback)
        {
            _onAttackCallback = onAttackCallback;
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketAction.Attack },
                { "channel", string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username) },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }

        private async void OnApplicationQuit()
        {
            await _ws.Close();
        }
    }
    
    public class SocketChannel
    {
        public const string DefaultChannel = "general";
        public const string DefaultChatChannel = "chat_general";
        public const string FightChannelSuffix = "fight_";
    }
    
    public class SocketAction
    {
        public const string Subscribe = "subscribe";
        public const string Unsubscribe = "unsubscribe";
        public const string GetBody = "message";
        public const string Message = "message";
        public const string MessageList = "messageList";
        public const string Equip = "equip";
        public const string UnEquip = "unEquip";
        public const string ShopList = "shopList";
        public const string RankList = "rankList";
        public const string StartFight = "fightStart";
        public const string Attack = "attack";
        public const string EndFight = "fightOver";
        public const string Error = "error";
    }
}
