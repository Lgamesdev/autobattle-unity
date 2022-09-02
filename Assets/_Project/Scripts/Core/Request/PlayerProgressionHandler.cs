using System;
using System.Collections;
using Core.Player;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class PlayerProgressionHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<PlayerProgression> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/progression",
                    UnityWebRequest.kHttpVerbGET,
                    error => { Debug.Log("Error on player progression load : " + error); },
                    response =>
                    {
                        //Debug.Log("Received player config : " + response);

                        PlayerProgression playerProgression = JsonConvert.DeserializeObject<PlayerProgression>(response);
                        setResult(playerProgression);
                    },
                    null,
                    GameManager.Instance.GetAuthentication())
                );
        }
    }
}