using System;
using System.Collections;
using System.Text;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Player
{
    public static class PlayerWalletHandler
    {
        public static IEnumerator Load(MonoBehaviour instance, Action<Currency[]> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/wallet",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on wallet load : " + error); },
                response =>
                {
                    //Debug.Log("Received currencies : " + response);

                    Currency[] currencies = JsonConvert.DeserializeObject<Currency[]>(response);

                    if (currencies != null)
                    {
                        //foreach (Currency currency in currencies) Debug.Log(currency.ToString());
                        setResult(currencies);
                    }
                    else
                    {
                        Debug.Log("currencies are null");
                    }
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        public static IEnumerator SaveWallet(MonoBehaviour instance, Currency currency)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(currency));

            yield return instance.StartCoroutine(RequestHandler.Request("api/user/wallet",
                UnityWebRequest.kHttpVerbPUT,
                error => { Debug.Log("Error on wallet save : " + error); },
                response =>
                {
                    Debug.Log("wallet saved : " + response);
                },
                bodyRaw,
                GameManager.Instance.GetAuthentication())
            );
        }
    }
}