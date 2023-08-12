using System;
using System.Collections;
using System.Text;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public static class PlayerWalletHandler
    {
        private static PlayerWalletManager _manager;
        
        public static /*IEnumerator*/void Load(MonoBehaviour instance, Action<string> onError, Action<Wallet> setResult)
        {
            /*yield return instance.StartCoroutine(RequestHandler.Request("api/user/wallet",
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
            );*/
        }

        public static /*IEnumerator*/void Buy(MonoBehaviour instance, Item item, Action<string> onError, Action<IBaseCharacterItem> onResult)
        {
            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/shop/buy/" + item.ID,
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    onError?.Invoke(error);
                },
                response =>
                {
                    //Debug.Log("buy reponse : " + response);
                    
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new BaseCharacterItemConverter());
                    
                    IBaseCharacterItem characterItem = JsonConvert.DeserializeObject<IBaseCharacterItem>(response, settings);
                    
                    onResult?.Invoke(characterItem);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
        
        public static /*IEnumerator*/void Sell(MonoBehaviour instance, IBaseCharacterItem characterItem, Action<string> onError, Action<string> onResult)
        {
            /*yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
            
            yield return instance.StartCoroutine(RequestHandler.Request("api/shop/sell/" + characterItem.Id,
                UnityWebRequest.kHttpVerbPUT,
                error =>
                {
                    onError?.Invoke(error);
                },
                response =>
                {
                    //Debug.Log("sell reponse : " + response);
                    onResult?.Invoke(response);
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
            
            yield return instance.StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());*/
        }
    }
}