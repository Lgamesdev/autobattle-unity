using System;
using System.Collections;
using LGamesDev.Fighting;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class FightHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<Fight> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/fight",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on fight load : " + error); },
                response =>
                {
                    //Debug.Log("Received fight : " + response);

                    Fight fight = JsonConvert.DeserializeObject<Fight>(response);

                    setResult(fight);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}