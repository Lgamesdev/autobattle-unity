using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LGamesDev.Core.Request;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Player
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

                    Inventory inventory = JsonConvert.DeserializeObject<Inventory>(response);

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

        public static IEnumerator Save(List<Item> items)
        {
            //var jsonString = Serialize(items);

            //Debug.Log("json array generated : \n" + jsonString);

            //PlayerPrefs.SetString("PlayerInventory", jsonString);

            yield return new WaitForEndOfFrame();
        }
    }
}