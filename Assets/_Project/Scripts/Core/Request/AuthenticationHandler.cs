using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Player;
using LGamesDev.Core.Player;
using Newtonsoft.Json;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public static class AuthenticationHandler
    {
        public static async /*Task<IEnumerator>*/ void PlatformConnect( /*MonoBehaviour instance,*/
            string username, /*string code,*/ Action<PlayerConfig> setResult)
        {
            Dictionary<string, object> requestParams = new Dictionary<string, object> { { "username", username } };

            PlayerConfig response = null;
            try
            {
                response =
                    await CloudCodeService.Instance.CallEndpointAsync<PlayerConfig>("Register", requestParams);
                
                Debug.Log("response : " + response);

                setResult(response);
            }
            catch (CloudCodeException e)
            {
                Debug.Log("error while calling cloud code register : " + e);
            }

            /*Dictionary<string, string> form = new Dictionary<string, string>() {
                {"username", username},
                {"code", code},
            };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/connect/google",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    if (error == AuthenticationError.PlatformConnectUsername)
                    {
                        AuthenticationManager.Instance.SetState(AuthenticationState.PlatformRegister);
                    }
                    else
                    {
                        GameManager.Instance.modalWindow.ShowAsTextPopup(
                            "Error",
                            error,
                            null,
                            "Ok",
                            null,
                            GameManager.Instance.modalWindow.Close
                        );
                    }
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

        public class AuthenticationError
        {
            public const string PlatformConnectUsername = "auth-0001";
        }
    }
}