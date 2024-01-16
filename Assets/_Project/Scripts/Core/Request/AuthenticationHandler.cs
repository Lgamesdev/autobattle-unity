using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core.Player;
using Newtonsoft.Json;
using NJsonSchema.Validation;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.Networking;
using JsonSchema = NJsonSchema.JsonSchema;

namespace LGamesDev.Core.Request
{
    public static class AuthenticationHandler
    {
        public static async void Connect(Action<PlayerConfig> onSuccess, Action<Exception> onFail, Action<Exception> onError)
        {
            try
            {
                // Call the function within the module and provide the parameters we defined in there
                string result = await CloudCodeService.Instance.CallModuleEndpointAsync<string>("PlayerModule", "Connect");
                //Debug.Log("connect result : " + result);

                if (ValidatePlayerConf(result))
                {
                    //Debug.Log("valid json schema : " + result);
                    PlayerConfig playerConf = JsonConvert.DeserializeObject<PlayerConfig>(result);
                    onSuccess?.Invoke(playerConf);
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
        
        public static async void Register(string username, Action<PlayerConfig> onSuccess, Action<Exception> onFail, Action<Exception> onError)
        {
            Dictionary<string, object> requestParams = new Dictionary<string, object> { { "username", username } };

            try
            {
                string result = await CloudCodeService.Instance.CallModuleEndpointAsync<string>("PlayerModule", "Register", requestParams);
                
                Debug.Log("register result : " + result);
                if (ValidatePlayerConf(result))
                {
                    Debug.Log("valid json schema : " + result);
                    PlayerConfig playerConf = JsonConvert.DeserializeObject<PlayerConfig>(result);
                    onSuccess?.Invoke(playerConf);
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
        
        private static bool ValidatePlayerConf(string json)
        {
            var schema = JsonSchema.FromType<PlayerConfig>();
            var schemaData = schema.ToJson();
            ICollection<ValidationError> errors = schema.Validate(json);

            /*foreach (var error in errors) {
                Debug.Log(error.Path + ": " + error.Kind);
            }*/

            return errors.Count <= 0;
        }
    }
}