using System;
using System.Collections.Generic;
using NJsonSchema.Validation;
using Unity.Services.CloudCode;
using UnityEngine;
using JsonSchema = NJsonSchema.JsonSchema;

namespace LGamesDev.Core.Request
{
    public class CharacterBodyHandler
    {
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<string> onError, Action<Body> setResult)
        {
            /*yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on player body load : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received player body : " + response);

                    var playerBody = JsonUtility.FromJson<Body>(response);
                    setResult(playerBody);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );*/
        }

        public static async void Save(Body body, Action<Exception> onError, Action<string> onSuccess)
        {
            Dictionary<string, object> requestParams = new Dictionary<string, object> { { "body", body } };
            
            try
            {
                string result = await CloudCodeService.Instance.CallModuleEndpointAsync<string>("PlayerModule", "CreationDone", requestParams);
                
                Debug.Log("creation done result : " + result);
                onSuccess?.Invoke(result);
                
                /*if (ValidatePlayerBody(result))
                {
                    Debug.Log("valid json schema : " + result);
                    //Body body = JsonConvert.DeserializeObject<Body>(result);
                    onSuccess?.Invoke(result);
                }
                else
                {
                    //AuthenticationException exception = JsonConvert.DeserializeObject<AuthenticationException>(result);
                    Debug.Log("on fail : " + result);
                    Exception e = JsonConvert.DeserializeObject<Exception>(result);
                    onFail?.Invoke(e);
                }*/
            }
            catch (Exception e)
            {
                Debug.Log("error while trying to cloud code connect : " + e.Message);

                onError?.Invoke(e);
            }
            
            /*var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            //Debug.Log("authentication : " + GameManager.Instance.GetAuthentication().ToString());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbPUT,
                onError,
                onResult,
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );*/
        }
        
        private static bool ValidatePlayerBody(string json)
        {
            var schema = JsonSchema.FromType<Body>();
            //var schemaData = schema.ToJson();
            ICollection<ValidationError> errors = schema.Validate(json);

            foreach (var error in errors) {
                Debug.LogError(error.Path + ": " + error.Kind);
            }
            
            return errors.Count <= 0;
        }
    }
}