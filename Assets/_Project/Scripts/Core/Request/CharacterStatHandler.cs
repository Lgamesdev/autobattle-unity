using System;
using System.Collections;
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
        public static IEnumerator LoadStats(MonoBehaviour instance, Action<string> onError, Action<Stat[]> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/stats",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on player stat loading : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received player stats : " + response);

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

        public static IEnumerator SaveStat(Stat newStat, Stat oldStat)
        {
            if (newStat != null)
            {
                //string jsonString = JsonConvert.SerializeObject(newItem, Formatting.Indented);
                //var jsonString = Serialize(newItem);

                //Debug.Log("json array generated : \n" + jsonString);

                //PlayerPrefs.SetString("PlayerEquipment_" + newItem.equipSlot, jsonString);
            }
            else
            {
                //PlayerPrefs.SetString("PlayerEquipment_" + oldItem.equipSlot, null);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}