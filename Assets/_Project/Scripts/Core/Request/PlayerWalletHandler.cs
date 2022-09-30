using System;
using System.Collections;
using System.Text;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public static class PlayerWalletHandler
    {
        private static PlayerWalletManager _manager;
        
        public static IEnumerator Load(MonoBehaviour instance, Action<string> onError, Action<Wallet> setResult)
        {
            yield return instance.StartCoroutine(RequestHandler.Request("api/user/wallet",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    onError?.Invoke("error on player wallet loading : \n" + error);
                },
                response =>
                {
                    //Debug.Log("Received currencies : " + response);

                    Wallet wallet = JsonConvert.DeserializeObject<Wallet>(response);

                    if (wallet != null)
                    {
                        //foreach (Currency currency in wallet.Currencies) Debug.Log(currency.ToString());
                        setResult(wallet);
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

        private static IEnumerator SaveWallet(MonoBehaviour instance, Currency currency)
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

        public static void SetManager(PlayerWalletManager manager)
        {
            _manager = manager;
            _manager.OnCurrencyChanged += OnCurrencyChanged;
        }

        private static void OnCurrencyChanged(Currency currency)
        {
            Debug.Log("theory saving");
            //_manager.StartCoroutine(SaveWallet(_manager, currency));
        }
    }
}