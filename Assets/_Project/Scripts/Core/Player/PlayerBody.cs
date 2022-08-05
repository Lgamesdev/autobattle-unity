using System;
using System.Collections;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Player
{
    public class PlayerBody
    {
        public int beardIndex;
        public int bodyIndex;
        public Color bodyPrimaryColor;
        public Color bodySecondaryColor;

        public Color hairColor;
        public int hairIndex;
        public Color skinColor;

        public static IEnumerator Load(MonoBehaviour instance, Action<PlayerBody> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on player body load : " + error); },
                response =>
                {
                    //Debug.Log("Received player body : " + response);

                    var playerBody = JsonUtility.FromJson<PlayerBody>(response);
                    setResult(playerBody);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}