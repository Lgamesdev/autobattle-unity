using System;
using System.Collections.Generic;
using System.Text;
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
                
                if (!Application.isPlaying) return;
                
                if (!_isError && isConnected)
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
                isConnected = false;
            };

            _ws.OnMessage += (bytes) =>
            {
                // getting the message as a string
                string response = Encoding.UTF8.GetString(bytes);
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
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    switch (socketMessage.Action)
                    {
                        case SocketReceiveAction.Initialisation:
                            InitialisationResult result =
                                JsonConvert.DeserializeObject<InitialisationResult>(socketMessage.Content);
                            Initialisation.Current.SetResult(result);
                            break;
                        
                        case SocketReceiveAction.TutorialDone:
                            Authentication authentication = GameManager.Instance.GetAuthentication();
                            authentication.PlayerConf.TutorialDone = true;
                            GameManager.Instance.SetAuthentication(authentication);
                            
                            GameManager.Instance.modalWindow.ShowAsTextPopup(
                                "Well Done!",
                                "Tutorial Done!",
                                "Ok",
                                null,
                                GameManager.Instance.modalWindow.Close
                            );
                            break;
                        
                        case SocketReceiveAction.Equip:
                            CharacterManager.Instance.equipmentManager.Equip(int.Parse(socketMessage.Content));
                            break;
                        
                        case SocketReceiveAction.UnEquip:
                            CharacterManager.Instance.equipmentManager.UnEquip((int)Enum.Parse<EquipmentSlot>(socketMessage.Content));
                            break;
                        
                        case SocketReceiveAction.AddStatPoint:
                            Stat stat = JsonConvert.DeserializeObject<Stat>(socketMessage.Content);
                            CharacterManager.Instance.statsManager.AddStatPoint(stat);
                            break;
                        
                        case SocketReceiveAction.ShopList:
                            settings.Converters.Add(new ItemConverter());
                            List<Item> shopItems = JsonConvert.DeserializeObject<List<Item>>(socketMessage.Content, settings);
                            ShopUI.Instance.ShopItems = shopItems;
                            break;
                        
                        case SocketReceiveAction.BuyItem:
                            settings.Converters.Add(new BaseCharacterItemConverter());
                            IBaseCharacterItem characterItem = JsonConvert.DeserializeObject<IBaseCharacterItem>(socketMessage.Content, settings);
                            CharacterManager.Instance.walletManager.BuyItem(characterItem);
                            break;
                        
                        case SocketReceiveAction.SellItem:
                            CharacterManager.Instance.walletManager.SellCharacterItem(int.Parse(socketMessage.Content));
                            break;
                        
                        case SocketReceiveAction.RankList:
                            List<Character> characterList = JsonConvert.DeserializeObject<List<Character>>(socketMessage.Content);
                            RankingUI.Instance.CharacterList = characterList;
                            break;
                        
                        case SocketReceiveAction.Error:
                            GameManager.Instance.modalWindow.ShowAsTextPopup(
                                "Error",
                                socketMessage.Content,
                                "Reconnect",
                                "Disconnect",
                                GameManager.Instance.LoadMainMenu,
                                GameManager.Instance.Logout,
                                GameManager.Instance.modalWindow.Close
                            );
                            break;
                    }
                    break;
                case SocketChannel.DefaultChatChannel:
                    switch (socketMessage.Action)
                    {
                        case SocketReceiveAction.MessageList:
                            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(socketMessage.Content);
                            if (messages != null)
                            {
                                foreach (Message m in messages)
                                {
                                    MessageReceived.Invoke(m);
                                }
                            }
                            break;
                        case SocketReceiveAction.Message:
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
                        case SocketReceiveAction.StartFight:
                            Fight fight = JsonConvert.DeserializeObject<Fight>(socketMessage.Content);
                            GameManager.Instance.LoadFight(fight);
                            break;
                        case SocketReceiveAction.Attack:
                            List<FightAction> fightActions = JsonConvert.DeserializeObject<List<FightAction>>(socketMessage.Content);
                            FightManager.Instance.Attack(fightActions);
                            break;
                        case SocketReceiveAction.ExperienceGained:
                            Dictionary<string, int> experiencePayload = JsonConvert.DeserializeObject<Dictionary<string, int>>(socketMessage.Content);
                            FightManager.Instance.playerCharacterFight.GetLevelSystem().AddExperience(
                                experiencePayload["level"], 
                                experiencePayload["oldExperience"], 
                                experiencePayload["aimedExperience"], 
                                experiencePayload["maxExperience"]
                            );
                            break;
                        case SocketReceiveAction.EndFight:
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
        
        //Default
        public void SubscribeToMainChannel()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySubscribe },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChannel }
            }));
        }
        
        //Tutorial
        public void TutorialFinished()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TutorialFinished },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }

        //Chat
        public void ConnectToChat()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));
        }
        
        public new async void SendMessage(string message)
        {
            if (_ws == null || string.IsNullOrEmpty(message)) return;
            
            if (_ws.State == WebSocketState.Open)
            {
                // Sending plain text to json
                await _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    { "action", SocketSendAction.SendMessage },
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
                { "action", SocketSendAction.TryUnsubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));
        }
        
        //Gear
        public void TryEquip(CharacterEquipment newEquipment)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryEquip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", newEquipment.id.ToString() }
            }));
        }
        
        public void TryUnEquip(CharacterEquipment characterEquipment)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryUnEquip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", characterEquipment.id.ToString() }
            }));
        }
        
        //Stats
        public void TryAddStatPoint(StatType statType)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryAddStatPoint },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", JsonConvert.SerializeObject(statType.ToString()) }
            }));
        }
        
        //Shop
        public void GetItemsList()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.GetShopList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }
        
        public void TryBuyItem(Item item)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryBuyItem },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", item.ID.ToString() }
            }));
        }
        
        public void TrySellItem(IBaseCharacterItem characterItem)
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySellItem },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", characterItem.Id.ToString() }
            }));
        }
        
        //Rank
        public void GetRankList()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.GetRankList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
        }
        
        //Fight
        public void SearchFight()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username) }
            }));
        }

        public void Attack()
        {
            _ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryAttack },
                { "channel", string.Concat(SocketChannel.FightChannelSuffix, GameManager.Instance.GetAuthentication().username) },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));
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
    }
    
    public class SocketChannel
    {
        public const string DefaultChannel = "general";
        public const string DefaultChatChannel = "chat_general";
        public const string FightChannelSuffix = "fight_";
    }
    
    public class SocketSendAction
    {
        public const string TrySubscribe = "trySubscribe";
        public const string TryUnsubscribe = "tryUnsubscribe";
        public const string TutorialFinished = "tutorialFinished";
        public const string SendMessage = "sendMessage";
        public const string TryEquip = "tryEquip";
        public const string TryUnEquip = "tryUnEquip";
        public const string TryAddStatPoint = "tryAddStatPoint";
        public const string GetShopList = "getShopList";
        public const string TryBuyItem = "tryBuyItem";
        public const string TrySellItem = "trySellItem";
        public const string GetRankList = "getRankList";
        public const string TryAttack = "tryAttack";
    }
    
    public class SocketReceiveAction
    {
        public const string Initialisation = "initialisation";
        public const string TutorialDone = "tutorialDone";
        public const string Message = "message";
        public const string MessageList = "messageList";
        public const string Equip = "equip";
        public const string UnEquip = "unEquip";
        public const string AddStatPoint = "addStatPoint";
        public const string ShopList = "shopList";
        public const string BuyItem = "buyItem";
        public const string SellItem = "sellItem";
        public const string RankList = "rankList";
        public const string StartFight = "fightStart";
        public const string Attack = "attack";
        public const string ExperienceGained = "experienceGained";
        public const string EndFight = "fightOver";
        public const string Error = "error";
    }
}
