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

        public static IEnumerator Register(MonoBehaviour instance, string username, string password, string email,
            Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                { "username", username },
                { "password", password },
                { "email", email }
            };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());

            yield return instance.StartCoroutine(RequestHandler.Request("api/register",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error",
                        error,
                        null,
                        "Ok",
                        null,
                        GameManager.Instance.modalWindow.Close
                    );
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);

                    setResult(authentication);
                },
                bodyRaw)
            );

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
        }

        public static IEnumerator Login(MonoBehaviour instance, string username, string password,
            Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                { "username", username },
                { "password", password },
            };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());

            yield return instance.StartCoroutine(RequestHandler.Request("api/login",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error",
                        error,
                        null,
                        "Ok",
                        null,
                        GameManager.Instance.modalWindow.Close
                    );
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);
                    setResult(authentication);
                },
                bodyRaw)
            );

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
        }

        public static /*IEnumerator*/
            void RefreshToken(MonoBehaviour instance, string refreshToken, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                { "refresh_token", refreshToken },
            };

            string bodyRequest = JsonConvert.SerializeObject(form);

            var bodyRaw = Encoding.UTF8.GetBytes(bodyRequest);

            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/token/refresh",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    //Debug.Log("Error : " + error);
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error",
                        error,
                        "Retry",
                        "Close",
                        () =>
                        {
                            instance.StartCoroutine(RefreshToken(instance, refreshToken, setResult));
                        },
                        () =>
                        {
                            GameManager.Instance.modalWindow.Close();
                            AuthenticationManager.Instance.SetState(AuthenticationState.Default);
                        });
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);
                    setResult(authentication);
                },
                bodyRaw)
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }

        public static /*IEnumerator*/ void Logout(MonoBehaviour instance, string refreshToken, Action onComplete)
        {
            /*Dictionary<string, string> form = new Dictionary<string, string>() {
                {"refresh_token", refreshToken},
            };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/token/invalidate",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    //Debug.Log("Error : " + error);
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error",
                        error,
                        null,
                        "Ok",
                        null,
                        GameManager.Instance.modalWindow.Close
                    );
                },
                _ =>
                {
                    //Debug.Log("Received : " + response);

                    onComplete?.Invoke();
                },
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
        }*/
        }

        public class AuthenticationException : Exception
        {
            public const string PlatformConnectUsername = "auth-0001";
        }
    }
}