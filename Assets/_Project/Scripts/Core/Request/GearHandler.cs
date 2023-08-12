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
    public class GearHandler
    {
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<string> onError, Action<Gear> setResult)
        {
            /*yield return instance.StartCoroutine(RequestHandler.Request("api/user/gear",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on equipments load : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received player equipments : " + response);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ItemConverter());

                    Gear gear = JsonConvert.DeserializeObject<Gear>(response, settings);

                    if (gear == null)
                    {
                        Debug.Log("No Equipment in response");
                    }
                    /*else
                    {
                        Debug.Log("gear : " + gear.ToString());
                    }#1#

                    setResult(gear);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );*/
        }

        public static /*IEnumerator*/void Equip(MonoBehaviour instance, CharacterEquipment newEquipment, Action<string> onError, Action<string> onResult)
        {
            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/gear/equip/" + newEquipment.id,
                UnityWebRequest.kHttpVerbPUT,
                error =>
                {
                    onError?.Invoke(error);
                },
                response =>
                {
                    onResult?.Invoke(response);
                },
                null,//bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
        
        public static /*IEnumerator*/void UnEquip(MonoBehaviour instance, CharacterEquipment oldEquipment, Action<string> onError, Action<string> onResult)
        {
            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/gear/unEquip/" + oldEquipment.id,
                UnityWebRequest.kHttpVerbPUT,
                error =>
                {
                    onError?.Invoke(error);
                },
                response =>
                {
                    onResult?.Invoke(response);
                },
                null,//bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
    }
}