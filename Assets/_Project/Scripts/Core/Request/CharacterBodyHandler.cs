using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class CharacterBodyHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<string> onError, Action<Body> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
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
            );
        }

        public static IEnumerator Save(MonoBehaviour instance, Body body, Action<string> onError, Action<string> onResult)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            //Debug.Log("authentication : " + GameManager.Instance.GetAuthentication().ToString());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbPUT,
                onError,
                onResult,
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}