using System;
using System.Collections.Generic;
using Core.Player;
using LGamesDev;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Fighting;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using Newtonsoft.Json;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Core.Network
{
    public class NetworkService: BaseNetworkService
    {
        //public override string channelName => "Default";
        
        //Socket Send Actions
        private const string TrySubscribe = "trySubscribe";
        private const string TryUnsubscribe = "tryUnsubscribe";
        private const string TryOpenLootBoxAction = "tryOpenLootBox";
        private const string TryEquipAction = "tryEquip";
        private const string TryUnEquipAction = "tryUnEquip";
        private const string TryAddStatPointAction = "tryAddStatPoint";
        private const string TutorialFinished = "tutorialFinished";
        
        //Todo
        private const string GetShopList = "getShopList";
        private const string TryBuyItemAction = "tryBuyItem";
        private const string TrySellItemAction = "trySellItem";
        
        private const string GetRankListAction = "getRankList";
        
        //Socket Received Actions
        private const string InitialisationAction = "initialisation";
        private const string TutorialDone = "tutorialDone";
        private const string OpenLootBox = "openLootBox";
        private const string Equip = "equip";
        private const string UnEquip = "unEquip";
        private const string AddStatPoint = "addStatPoint";
        private const string StartFight = "fightStart";
        private const string Error = "error";
        
        //Todo
        private const string ShopList = "shopList";
        private const string BuyItem = "buyItem";
        private const string SellItem = "sellItem";
        private const string RankList = "rankList";

        protected override IDisposable Cancellation { get; set; }
        
        private void Start()
        {
            //Subscribe(GameManager.Instance.networkManager);
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
            JsonSerializerSettings settings = new JsonSerializerSettings();
            switch (socketMessage.Channel)
            {
                case DefaultChannel:
                    //Debug.Log("new socket message for networkService : " + socketMessage);
                    IBaseCharacterItem characterItem;
                    switch (socketMessage.Action)
                    {
                        case InitialisationAction:
                            InitialisationResult result =
                                JsonConvert.DeserializeObject<InitialisationResult>(socketMessage.Content);
                            //Initialisation.Current.SetResult(result);
                            break;
                        
                        case TutorialDone:
                            PlayerConfig playerConfig = GameManager.Instance.GetPlayerConf();
                            playerConfig.TutorialDone = true;
                            GameManager.Instance.SetPlayerConf(playerConfig);
                            
                            GameManager.Instance.modalWindow.ShowAsTextPopup(
                                "Well Done!",
                                "Tutorial Done!",
                                "Ok",
                                null,
                                GameManager.Instance.modalWindow.Close
                            );
                            break;
                        
                        case OpenLootBox:
                            //Debug.Log("open lootbox : " + socketMessage.Content);
                            settings.Converters.Add(new BaseCharacterItemConverter());

                            CharacterLootBox lootBox =
                                JsonConvert.DeserializeObject<CharacterLootBox>(socketMessage.Content, settings);

                            foreach (IBaseCharacterItem rewardItem in lootBox.Reward.Items)
                            {
                                PlayerInventoryManager.Instance.AddItem(rewardItem);
                            }
                            
                            PlayerInventoryManager.Instance.RemoveItem(lootBox);
                            break;
                        
                        case Equip:
                            CharacterManager.Instance.equipmentManager.Equip(socketMessage.Content);
                            break;
                        
                        case UnEquip:
                            CharacterManager.Instance.equipmentManager.UnEquip((int)Enum.Parse<EquipmentSlot>(socketMessage.Content));
                            break;
                        
                        case AddStatPoint:
                            Stat stat = JsonConvert.DeserializeObject<Stat>(socketMessage.Content);
                            CharacterManager.Instance.statsManager.AddStatPoint(stat);
                            break;
                        
                        case ShopList:
                            settings.Converters.Add(new ItemConverter());
                            List<ShopPurchase> shopItems = JsonConvert.DeserializeObject<List<ShopPurchase>>(socketMessage.Content, settings);
                            ShopUI.Instance.ShopItems = shopItems;
                            break;
                        
                        case BuyItem:
                            settings.Converters.Add(new BaseCharacterItemConverter());
                            characterItem = JsonConvert.DeserializeObject<IBaseCharacterItem>(socketMessage.Content, settings);
                            CharacterManager.Instance.walletManager.BuyItem(characterItem);
                            break;
                        
                        case SellItem:
                            CharacterManager.Instance.walletManager.SellCharacterItem(socketMessage.Content);
                            break;
                        
                        case RankList:
                            List<Character> characterList = JsonConvert.DeserializeObject<List<Character>>(socketMessage.Content);
                            RankingUI.Instance.CharacterList = characterList;
                            break;

                        case Error:
                            Debug.Log("error : " + socketMessage.Content);
                            GameManager.Instance.modalWindow.ShowAsTextPopup(
                                "Error",
                                socketMessage.Content,
                                "Reconnect",
                                "Disconnect",
                                () =>
                                {
                                    Loader.Load(Loader.Scene.MenuScene);
                                },
                                GameManager.Instance.Logout,
                                GameManager.Instance.modalWindow.Close
                            );
                            break;
                    }
                    break;
                /*case var value when string.Equals(value, string.Concat(FightChannelSuffix, GameManager.Instance.GetAuthentication().username)):
                    switch (socketMessage.Action) {
                        case StartFight:
                            //Debug.Log("start fight : " + socketMessage);
                            settings.Converters.Add(new FighterConverter());
                            Fight fight = JsonConvert.DeserializeObject<Fight>(socketMessage.Content, settings);
                            GameManager.Instance.LoadFight(fight);
                            break;
                    }
                    break;*/
            }
        }
        
        //Subscriptions
        public async void GetPlayerInfos()
        {
            InitialisationResult[] results =
                await CloudCodeService.Instance.CallEndpointAsync<InitialisationResult[]>("GetPlayerInfos");

            foreach (InitialisationResult result in results)
            {
                //Initialisation.Current.SetResult(result);
            }
        }
        
        public void SubscribeToMainChannel()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TrySubscribe,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = DefaultChannel,
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", TrySubscribe },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", DefaultChannel }
            }));*/
        }
        
        public void ConnectToChat()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TrySubscribe,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = DefaultChatChannel,
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));*/
        }

        public void SearchFight(FightType fightType)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TrySubscribe,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = string.Concat( 
                    FightChannelSuffix, 
                    GameManager.Instance.GetAuthentication().username
                ),
                Type = fightType.ToString()
            });*/
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", string.Concat( 
                    SocketChannel.FightChannelSuffix, 
                    GameManager.Instance.GetAuthentication().username
                )},
                { "type", fightType.ToString()}
            }));*/
        }
        
        //UnSubscribe
        public void ExitChat()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryUnsubscribe,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = DefaultChatChannel
            });*/
            /*_ws?.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryUnsubscribe },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", SocketChannel.DefaultChatChannel }
            }));*/
        }

        //Tutorial
        public void OnTutorialFinished()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TutorialFinished,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TutorialFinished },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));*/
        }
        
        //Stats
        public void TryAddStatPoint(StatType statType)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryAddStatPointAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = JsonConvert.SerializeObject(statType.ToString())
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", TryAddStatPointAction },
                { "channel", DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", JsonConvert.SerializeObject(statType.ToString()) }
            }));*/
        }
        
        public void TryOpenLootBox(IBaseCharacterItem characterItem)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryOpenLootBoxAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = characterItem.Id.ToString(),
            });*/
        }
        
        //Gear
        public void TryEquip(CharacterEquipment newEquipment)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryEquipAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = newEquipment.id.ToString(),
            });*/
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryEquip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", newEquipment.id.ToString() }
            }));*/
        }
        
        public void TryUnEquip(CharacterEquipment characterEquipment)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryUnEquipAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = characterEquipment.id.ToString(),
            });*/
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryUnEquip },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", characterEquipment.id.ToString() }
            }));*/
        }
        
        //Shop
        public void GetItemsList()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = GetShopList,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.GetShopList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));*/
        }
        
        public void TryBuyItem(Item item)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TryBuyItemAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = item.ID.ToString()
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TryBuyItem },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", item.ID.ToString() }
            }));*/
        }
        
        public void TrySellItem(IBaseCharacterItem characterItem)
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = TrySellItemAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username,
                Content = characterItem.Id.ToString()
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.TrySellItem },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
                { "content", characterItem.Id.ToString() }
            }));*/
        }
        
        //Rank
        public void GetRankList()
        {
            /*GameManager.Instance.networkManager.SendSocket(new SocketMessage()
            {
                Action = GetRankListAction,
                Channel = DefaultChannel,
                Username = GameManager.Instance.GetAuthentication().username
            });*/
            
            /*_ws.SendText(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "action", SocketSendAction.GetRankList },
                { "channel", SocketChannel.DefaultChannel },
                { "username", GameManager.Instance.GetAuthentication().username },
            }));*/
        }
        
        protected override void OnDestroy()
        {
            Cancellation.Dispose();
        }
    }
}