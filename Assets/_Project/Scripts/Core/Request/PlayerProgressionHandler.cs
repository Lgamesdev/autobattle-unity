﻿using System;
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
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<string> onError, Action<PlayerProgression> setResult)
        {
            /*yield return instance.StartCoroutine(RequestHandler.Request("api/user/progression",
                    UnityWebRequest.kHttpVerbGET,
                    error =>
                    {
                        onError?.Invoke("error on playerProgression load : \n" + error);
                    },
                    response =>
                    {
                        //Debug.Log("Received player config : " + response);

                        PlayerProgression playerProgression = JsonConvert.DeserializeObject<PlayerProgression>(response);
                        setResult(playerProgression);
                    },
                    null,
                    GameManager.Instance.GetAuthentication())
                );*/
        }
    }
}