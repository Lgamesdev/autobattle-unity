using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NJsonSchema.Validation;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class GearHandler
    {
        public static async void Equip(CharacterEquipment newEquipment, Action onSuccess, Action<Exception> onFail, Action<Exception> onError)
        {
            Dictionary<string, object> requestParams = new Dictionary<string, object> { { "characterEquipment", newEquipment } };
            
            try
            {
                // Call the function within the module and provide the parameters we defined in there
                string result =
                    await CloudCodeService.Instance.CallModuleEndpointAsync<string>("PlayerModule", "Equip", requestParams);
                Debug.Log("equip result : " + result);

                if (ValidateCharacterEquipment(result))
                {
                    //Debug.Log("valid json schema : " + result);
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                    CharacterEquipment characterEquipment = JsonConvert.DeserializeObject<CharacterEquipment>(result, jsonSerializerSettings);
                    
                    onSuccess?.Invoke();
                }
                else
                {
                    //AuthenticationException exception = JsonConvert.DeserializeObject<AuthenticationException>(result);
                    Debug.Log("on fail : " + result);
                    Exception e = JsonConvert.DeserializeObject<Exception>(result);
                    onFail?.Invoke(e);
                }
            }
            catch (Exception e)
            {
                Debug.Log("error while trying to cloud code connect : " + e.Message);

                onError?.Invoke(e);
            }
        }
        
        public static /*IEnumerator*/void UnEquip(MonoBehaviour instance, CharacterEquipment oldEquipment, Action<string> onError, Action<string> onResult)
        {
            /*yield return instance.StartCoroutine(StartManager.Instance.loadingScreen.EnableWaitingScreen());
            
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
                StartManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(StartManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
        
        private static bool ValidateCharacterEquipment(string json)
        {
            var schema = JsonSchema.FromType<CharacterEquipment>();
            var schemaData = schema.ToJson();
            ICollection<ValidationError> errors = schema.Validate(json);

            foreach (var error in errors) {
                Debug.Log("error in validate initialisation : " + error);
            }

            return errors.Count <= 0;
        }
    }
}