using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LGamesDev.Core.Player;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public static class AuthenticationHandler
    {
        public static IEnumerator PlatformRegister(MonoBehaviour instance, string token, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"token", token},
            };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/tokenRegister",
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
        
        public static IEnumerator Register(MonoBehaviour instance, string username, string password, string email, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"username", username},
                {"password", password}, 
                {"email", email}
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
        
        public static IEnumerator Login(MonoBehaviour instance, string username, string password, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"username", username},
                {"password", password},
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
        
        public static IEnumerator RefreshToken(MonoBehaviour instance, string refreshToken, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"refresh_token", refreshToken},
            };

            string bodyRequest = JsonConvert.SerializeObject(form);

            var bodyRaw = Encoding.UTF8.GetBytes(bodyRequest);

            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/token/refresh",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    //Debug.Log("Error : " + error);
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error",
                        error,
                        "Retry",
                        "Disconnect",
                        () =>
                        {
                            instance.StartCoroutine(RefreshToken(instance, refreshToken, setResult));
                        },
                        GameManager.Instance.Logout
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

        public static IEnumerator Logout(MonoBehaviour instance, string refreshToken, Action onComplete)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
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
        }
    }
}