using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class CharacterStatHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<string> onError, Action<Stat[]> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/stats",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on player stat loading : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received character stats : " + response);

                    Stat[] responseRaw =
                        JsonConvert.DeserializeObject<Stat[]>(response);

                    if (responseRaw == null)
                    {
                        Debug.Log("No Stats in response");
                    }
                    
                    /*string log = "stats [ \n";
                    foreach (Stat stat in responseRaw) log += stat.ToString() + "\n";
                    Debug.Log(log + "\n ]");*/
                    
                    setResult(responseRaw);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        public static IEnumerator AddStatPoint(MonoBehaviour instance, StatType statType, Action<string> onError, Action<string> onResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"statType", statType.ToString()},
            };
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/stats/add",
                UnityWebRequest.kHttpVerbPUT,
                error =>
                {
                    onError?.Invoke(error);
                },
                response =>
                {
                    onResult?.Invoke(response);
                },
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
        }
    }
}