using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class PlayerBodyHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<Body> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on player body load : " + error); },
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
    }
}