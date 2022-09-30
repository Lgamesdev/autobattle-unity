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
    public class CharacterEquipmentHandler
    {
        public static IEnumerator LoadEquipments(MonoBehaviour instance, Action<string> onError, Action<CharacterEquipment[]> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/equipments",
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

                    CharacterEquipment[] responseRaw =
                        JsonConvert.DeserializeObject<CharacterEquipment[]>(response, settings);

                    if (responseRaw == null)
                    {
                        Debug.Log("No Equipment in response");
                    }
                    /*else
                    {
                        string log = "equipments [ \n";
                        foreach (CharacterEquipment equipment in responseRaw) log += equipment.ToString() + "\n";
                        Debug.Log(log + "\n ]");
                    }*/

                    setResult(responseRaw);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        public static IEnumerator Equip(MonoBehaviour instance, CharacterEquipment newEquipment, Action<string> onError, Action<string> onResult)
        {
            /*Dictionary<string, Equipment> form = new Dictionary<string, Equipment>() {
                {"newEquipment", newEquipment},
                {"oldEquipment", oldEquipment},
            };*/

            /*JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ItemConverter());

            string json = JsonConvert.SerializeObject(newEquipment, settings);
            
            Debug.Log(json);
            
            var bodyRaw = Encoding.UTF8.GetBytes(json);*/
            
            Debug.Log("newEquipment : " + newEquipment.ToString());

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/equipments/equip/" + newEquipment.id,
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
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
        }
    }
}