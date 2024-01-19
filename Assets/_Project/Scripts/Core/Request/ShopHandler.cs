using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LGamesDev.Core.Character;
using LGamesDev.Fighting;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NJsonSchema.Validation;
using Unity.Services.CloudCode;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class ShopHandler
    {
        public static async void BuyItem(string transactionId, Action onSuccess, Action<Exception> onFail,
            Action<Exception> onError)
        {
            Dictionary<string, object> requestParams = new Dictionary<string, object> { { "transactionId", transactionId } };
            
            try
            {
                // Call the function within the module and provide the parameters we defined in there
                string purchaseResult = 
                    await CloudCodeService.Instance.CallModuleEndpointAsync<string>("PlayerModule", "BuyItem", requestParams);
                
                onSuccess?.Invoke();
                //Debug.Log("purchase result : " + purchaseResult);
                
                /*if (purchaseResult != null)
                {
                    List<ShopPurchase> shopList = new List<ShopPurchase>();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ItemDataConverter());

                    foreach (var virtualPurchaseDefinition in shopListResult)
                    {
                        int cost = virtualPurchaseDefinition.Costs[0].Amount;
                        var itemConf = virtualPurchaseDefinition.Rewards[0].Item.GetReferencedConfigurationItem();
                        //Debug.Log("item name : " + itemConf.Name + "\n conf result : " + itemConf.CustomDataDeserializable.GetAsString());
                        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(itemConf.CustomDataDeserializable.GetAsString(), settings);
                        Item item = null;

                        if (itemData.GetType() == typeof(ItemData))
                        {
                            item = new Item()
                            {
                                ID = itemConf.Id,
                                cost = cost,
                                name = itemConf.Name,
                                itemQuality = itemData.itemQuality,
                                icon = itemData.icon,
                                itemType = itemData.itemType
                            };
                        } 
                        else if (itemData.GetType() == typeof(EquipmentData))
                        {
                            EquipmentData equipmentData = itemData as EquipmentData;
                            item = new Equipment()
                            {
                                ID = itemConf.Id,
                                cost = cost,
                                name = itemConf.Name,
                                itemQuality = equipmentData.itemQuality,
                                icon = equipmentData.icon,
                                itemType = equipmentData.itemType,
                                stats = equipmentData.stats,
                                equipmentSlot = equipmentData.EquipmentSlot,
                                requiredLevel = equipmentData.requiredLevel,
                                spriteId = equipmentData.spriteId
                            };
                        }
                        shopList.Add(new ShopPurchase()
                        {
                            ID = virtualPurchaseDefinition.Id,
                            item = item
                        });
                    }
                    //Debug.Log("valid json schema : " + result);
                    /*var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                    List<Item> shopItemResult = JsonConvert.DeserializeObject<List<Item>>(shopListResult, jsonSerializerSettings);#1#
                    
                    onSuccess?.Invoke(shopList);
                }
                else
                {
                    //AuthenticationException exception = JsonConvert.DeserializeObject<AuthenticationException>(result);
                    Debug.Log("on fail : " + shopListResult);
                    onFail?.Invoke(new Exception("Shop list is null"));
                }*/
            }
            catch (Exception e)
            {
                Debug.Log("error while trying to cloud code shopList : " + e.Message);

                onError?.Invoke(e);
            }
        }
        
        public static void GetPurchase(Action<List<ShopPurchase>> onSuccess, Action<Exception> onFail,
            Action<Exception> onError)
        {
            try
            {
                // Call the function within the module and provide the parameters we defined in there
                List<VirtualPurchaseDefinition> shopListResult = EconomyService.Instance.Configuration.GetVirtualPurchases();
                
                if (shopListResult != null)
                {
                    List<ShopPurchase> shopList = new List<ShopPurchase>();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ItemDataConverter());

                    foreach (var virtualPurchaseDefinition in shopListResult)
                    {
                        int cost = virtualPurchaseDefinition.Costs[0].Amount;
                        var itemConf = virtualPurchaseDefinition.Rewards[0].Item.GetReferencedConfigurationItem();
                        //Debug.Log("item name : " + itemConf.Name + "\n conf result : " + itemConf.CustomDataDeserializable.GetAsString());
                        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(itemConf.CustomDataDeserializable.GetAsString(), settings);
                        Item item = null;

                        if (itemData.GetType() == typeof(ItemData))
                        {
                            item = new Item()
                            {
                                ID = itemConf.Id,
                                cost = cost,
                                name = itemConf.Name,
                                itemQuality = itemData.itemQuality,
                                icon = itemData.icon,
                                itemType = itemData.itemType
                            };
                        } 
                        else if (itemData.GetType() == typeof(EquipmentData))
                        {
                            EquipmentData equipmentData = itemData as EquipmentData;
                            item = new Equipment()
                            {
                                ID = itemConf.Id,
                                cost = cost,
                                name = itemConf.Name,
                                itemQuality = equipmentData.itemQuality,
                                icon = equipmentData.icon,
                                itemType = equipmentData.itemType,
                                stats = equipmentData.stats,
                                equipmentSlot = equipmentData.EquipmentSlot,
                                requiredLevel = equipmentData.requiredLevel,
                                spriteId = equipmentData.spriteId
                            };
                        }
                        shopList.Add(new ShopPurchase()
                        {
                            ID = virtualPurchaseDefinition.Id,
                            item = item
                        });
                    }
                    //Debug.Log("valid json schema : " + result);
                    /*var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                    List<Item> shopItemResult = JsonConvert.DeserializeObject<List<Item>>(shopListResult, jsonSerializerSettings);*/
                    
                    onSuccess?.Invoke(shopList);
                }
                else
                {
                    //AuthenticationException exception = JsonConvert.DeserializeObject<AuthenticationException>(result);
                    Debug.Log("on fail : " + shopListResult);
                    onFail?.Invoke(new Exception("Shop list is null"));
                }
            }
            catch (Exception e)
            {
                Debug.Log("error while trying to cloud code shopList : " + e.Message);

                onError?.Invoke(e);
            }
        }
        
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<List<Item>> setResult)
        {
            /*yield return instance.StartCoroutine(StartManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/shop",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /shop : " + error); },
                response =>
                {
                    //Debug.Log("Received /shop : " + response);
                    
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ItemConverter());
                    
                    List<Item> shopItems = JsonConvert.DeserializeObject<List<Item>>(response, settings);

                    setResult(shopItems);
                },
                null,
                StartManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(StartManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
    }
}