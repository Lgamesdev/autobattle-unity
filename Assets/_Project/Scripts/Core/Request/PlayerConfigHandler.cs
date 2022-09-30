using System;
using System.Collections;
using Core.Player;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class PlayerConfigHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<string> onError, Action<PlayerConfig> setResult)
        {
            PlayerConfig playerConf = JsonConvert.DeserializeObject<PlayerConfig>(PlayerPrefs.GetString("PlayerConfig"));
            if (playerConf != null)
            {
                setResult(playerConf);
                yield return new WaitForEndOfFrame();
            } else {
                yield return instance.StartCoroutine(RequestHandler.Request("api/user/configuration",
                    UnityWebRequest.kHttpVerbGET,
                    error =>
                    {
                        onError?.Invoke("error on player config loading : \n" + error);
                    },
                    response =>
                    {
                        //Debug.Log("Received player config : " + response);

                        playerConf = JsonConvert.DeserializeObject<PlayerConfig>(response);
                        setResult(playerConf);
                    },
                    null,
                    GameManager.Instance.GetAuthentication())
                );
            }
        }
    }
}