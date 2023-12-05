using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Validation;
using Unity.Services.CloudCode;
using UnityEngine;
using JsonSchema = NJsonSchema.JsonSchema;

namespace LGamesDev.Core.Request
{
    public static class TutorialHandler
    {
        public static async void TutorialFinished(/*Action<Exception> onFail,*/ Action<Exception> onError)
        {
            try
            {
                // Call the function within the module and provide the parameters we defined in there
                string result =
                    await CloudCodeService.Instance.CallModuleEndpointAsync("PlayerModule", "TutorialFinished");
                Debug.Log("connect result : " + result);

                /*if (ValidateInitialisation(result))
                {
                    Debug.Log("valid json schema : " + result);
                    
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                    List<InitialisationResult> initialisationResults = JsonConvert.DeserializeObject<List<InitialisationResult>>(result, jsonSerializerSettings);
                    onSuccess?.Invoke(initialisationResults);
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
        }
        
        private static bool ValidateInitialisation(string json)
        {
            var schema = JsonSchema.FromType<List<InitialisationResult>>();
            var schemaData = schema.ToJson();
            ICollection<ValidationError> errors = schema.Validate(json);

            foreach (var error in errors) {
                Debug.Log("error in validate initialisation : " + error);
            }

            return errors.Count <= 0;
        }
    }
}