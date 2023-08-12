using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Fighting;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class ShopHandler
    {
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<List<Item>> setResult)
        {
            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
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
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
    }
}