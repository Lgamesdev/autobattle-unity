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
    public class CharacterEquipmentHandler
    {
        public static IEnumerator LoadEquipments(MonoBehaviour instance, Action<CharacterEquipment[]> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/equipments",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on equipments load : " + error); },
                response =>
                {
                    Debug.Log("Received player equipments : " + response);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new EquipmentConverter());

                    CharacterEquipment[] equipments = JsonConvert.DeserializeObject<CharacterEquipment[]>(response, settings);
                    
                    if (equipments != null)
                    {
                        string log = "equipments [ \n";
                        foreach (CharacterEquipment equipment in equipments) log += equipment.ToString() + "\n";
                        Debug.Log(log + "\n ]");
                        setResult(equipments);
                    } else {
                        Debug.Log("character equipments null");
                    }
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        public static IEnumerator SaveEquipment(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
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