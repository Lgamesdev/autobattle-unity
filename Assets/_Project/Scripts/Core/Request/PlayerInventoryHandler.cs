using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class PlayerInventoryHandler
    {
        public async void GetInventory(Action<Inventory> onSuccess, Action<Exception> onFail, Action<Exception> onError)
        {
            
        }
        
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<string> onError, Action<Inventory> setResult)
        {
            /*yield return instance.StartCoroutine(RequestHandler.Request("api/user/inventory",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on inventory load : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received inventory : " + response);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new BaseCharacterItemConverter());
                    
                    Inventory inventory = JsonConvert.DeserializeObject<Inventory>(response, settings);

                    if (inventory != null) {
                        //Debug.Log(inventory.ToString());
                        setResult(inventory);
                    } else {
                        Debug.Log("inventory is null");
                    }
                    
                },
                null,
                StartManager.Instance.GetAuthentication())
            );*/
        }

        public static /*IEnumerator*/void Save(List<Equipment> items)
        {
            //var jsonString = Serialize(items);

            //Debug.Log("json array generated : \n" + jsonString);

            //PlayerPrefs.SetString("PlayerInventory", jsonString);

            /*yield return new WaitForEndOfFrame();*/
        }
    }
}