using System;
using System.Collections;
using System.Net;
using System.Text;
using LGamesDev.Core.Authentication;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev
{
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance;

        public delegate void AuthenticationError(string message);

        public AuthenticationError OnAuthenticationError;

        private GameManager _gameManager;

        private void Awake()
        {
            Instance = this;

            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            if (_gameManager.GetAuthentication() == null)
            {
                Debug.Log("no credentials");
                StartCoroutine(_gameManager.DisableLoadingScreen());
            }
            else
            {
                Debug.Log("credentials exists : " + _gameManager.GetAuthentication().user);
                //StartCoroutine(Refresh(gameManager.GetAuthentication().refresh_token));
            }
        }

        public void Submit(AuthenticationState authenticationState, string username = "", string password = "",
            string email = "", string refreshToken = "")
        {
            switch (authenticationState)
            {
                case AuthenticationState.Register:
                    Register(username, password, email);
                    break;
                case AuthenticationState.Login:
                    Login(username, password);
                    break;
                case AuthenticationState.Refresh:
                    Refresh(refreshToken);
                    break;
            }
        }

        private void Register(string username, string password, string email)
        {
            var form = new Register { username = username, password = password, email = email };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(form));

            StartCoroutine(RequestHandler.Request("api/register",
                UnityWebRequest.kHttpVerbPOST,
                error =>
                {
                    Debug.Log("Error : " + error);
                    OnAuthenticationError?.Invoke(error);

                    StartCoroutine(_gameManager.DisableLoadingScreen());
                },
                response =>
                {
                    Debug.Log("Received : " + response);

                    _gameManager.SetAuthentication(JsonConvert.DeserializeObject<Authentication>(response));

                    StartCoroutine(_gameManager.LoadGame());
                },
                bodyRaw)
            );
        }

        private void Login(string username, string password)
        {
            var form = new Login { username = username, password = password };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(form));

            StartCoroutine(RequestHandler.Request("api/login",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    Debug.Log("Error : " + error);
                    OnAuthenticationError?.Invoke(error);

                    StartCoroutine(_gameManager.DisableLoadingScreen());
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    _gameManager.SetAuthentication(JsonConvert.DeserializeObject<Authentication>(response));

                    StartCoroutine(_gameManager.LoadGame());
                },
                bodyRaw)
            );
        }

        private void Refresh(string refreshToken)
        {
            var form = new Refresh { refresh_token = refreshToken };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(form));

            StartCoroutine(RequestHandler.Request("api/token/refresh",
                UnityWebRequest.kHttpVerbGET,
                error =>
                {
                    Debug.Log("Error : " + error);
                    OnAuthenticationError?.Invoke(error);

                    StartCoroutine(_gameManager.DisableLoadingScreen());
                },
                response =>
                {
                    Debug.Log("Received : " + response);

                    _gameManager.SetAuthentication(JsonConvert.DeserializeObject<Authentication>(response));

                    StartCoroutine(_gameManager.LoadGame());
                },
                bodyRaw)
            );

            /*if (request.result == UnityWebRequest.Result.Success)
            {
                switch (request.responseCode)
                {
                    case (int)HttpStatusCode.OK:
                        yield return new WaitForEndOfFrame();
                        _gameManager.SetAuthentication(JsonConvert.DeserializeObject<Authentication>(request.downloadHandler.text));

                        yield return new WaitForEndOfFrame();
                        StartCoroutine(_gameManager.LoadGame());
                        yield break;
                    case (int)HttpStatusCode.Unauthorized:
                        StartCoroutine(_gameManager.DisableLoadingScreen());
                        /*JObject data = JsonConvert.DeserializeObject<JObject>(request.downloadHandler.text);

                    UI_Authentication.ShowLoginError("Error has occured : " + request.responseCode
                        + "\n result : " + request.result
                        + "\n " + data["title"].Value<string>() + " " + data["detail"].Value<string>());#1#
                        break;
                    default:
                        StartCoroutine(_gameManager.DisableLoadingScreen());
                        OnAuthenticationError?.Invoke("An error has occured, please try to reconnect manually");
                        break;
                }
            }
            else
            {
                StartCoroutine(_gameManager.DisableLoadingScreen());

                OnAuthenticationError?.Invoke("Error has occured : " + request.responseCode
                                                                     + "\n result : " + request.result
                                                                     + "\n error : " + request.error
                );
            }*/
        }
    }
}

public enum AuthenticationState
{
    Default,
    Register,
    Login,
    Refresh
}