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
        public static IEnumerator Register(MonoBehaviour instance, string username, string password, string email, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"username", username},
                {"password", password}, 
                {"email", email}
            };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/register",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    Debug.Log("Error : " + error);
                },
                response =>
                {
                    Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);
                    
                    setResult(authentication);
                },
                bodyRaw)
            );
        }
        
        public static IEnumerator Login(MonoBehaviour instance, string username, string password, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"username", username},
                {"password", password},
            };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            

            yield return instance.StartCoroutine(RequestHandler.Request("api/login",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    Debug.Log("Error : " + error);
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);
                    setResult(authentication);
                },
                bodyRaw)
            );
        }
        
        public static IEnumerator RefreshToken(MonoBehaviour instance, string refreshToken, Action<Authentication> setResult)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"refresh_token", refreshToken},
            };

            string bodyRequest = JsonConvert.SerializeObject(form);

            var bodyRaw = Encoding.UTF8.GetBytes(bodyRequest);

            yield return instance.StartCoroutine(RequestHandler.Request("api/token/refresh",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    //Debug.Log("Error : " + error);
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    Authentication authentication = JsonConvert.DeserializeObject<Authentication>(response);
                    setResult(authentication);
                },
                bodyRaw)
            );
        }

        public static IEnumerator Logout(MonoBehaviour instance, string refreshToken, Action onComplete)
        {
            Dictionary<string, string> form = new Dictionary<string, string>() {
                {"refresh_token", refreshToken},
            };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(form));
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/token/invalidate",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    Debug.Log("Error : " + error);
                },
                _ =>
                {
                    //Debug.Log("Received : " + response);

                    onComplete?.Invoke();
                },
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}