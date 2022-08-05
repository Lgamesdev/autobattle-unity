using System;
using System.Collections;
using Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Player
{
    public class PlayerConfigHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<PlayerConfig> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/infos",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on player config load : " + error); },
                response =>
                {
                    //Debug.Log("Received player config : " + response);

                    PlayerConfig playerInfos = JsonUtility.FromJson<PlayerConfig>(response);
                    setResult(playerInfos);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}