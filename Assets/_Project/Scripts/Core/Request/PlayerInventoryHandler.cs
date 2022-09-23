using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class PlayerInventoryHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<Inventory> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/inventory",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on inventory load : " + error); },
                response =>
                {
                    //Debug.Log("Received inventory : " + response);
                    
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ItemConverter());
                    
                    Inventory inventory = JsonConvert.DeserializeObject<Inventory>(response, settings);

                    if (inventory != null) {
                        //Debug.Log(inventory.ToString());
                        setResult(inventory);
                    } else {
                        Debug.Log("inventory is null");
                    }
                    
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        public static IEnumerator Save(List<Equipment> items)
        {
            //var jsonString = Serialize(items);

            //Debug.Log("json array generated : \n" + jsonString);

            //PlayerPrefs.SetString("PlayerInventory", jsonString);

            yield return new WaitForEndOfFrame();
        }
    }
}