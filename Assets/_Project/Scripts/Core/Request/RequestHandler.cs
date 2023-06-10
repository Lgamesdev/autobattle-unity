using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace LGamesDev.Core.Request
{
    public class RequestHandler : MonoBehaviour
    {
        private const string BaseURL = @"http://autobattle.hopto.org:35080/";

        public delegate void RequestErrorEvent(string error);
        public static RequestErrorEvent RequestError;

        public static IEnumerator Request(string url, string httpVerb, Action<string> onError, Action<string> onSuccess,
            byte[] bodyRaw = null,
            Authentication authentication = null)
        {
            using var request = new UnityWebRequest(BaseURL + url, httpVerb);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            if (authentication != null) request.SetRequestHeader("Authorization", $"Bearer {authentication.token}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess(request.downloadHandler.text);
            } 
            else 
            {
                //string error = request.error + "\n";
                string error;

                if (!string.IsNullOrEmpty(request.downloadHandler.text))
                {
                    try
                    {
                        JObject parsedJObjet = JObject.Parse(request.downloadHandler.text);

                        error = parsedJObjet["detail"]?.ToString();
                        error += parsedJObjet["message"]?.ToString();
                    }
                    catch (JsonReaderException)
                    {
                        error = "handling error by copying it to clipboard";
                        GUIUtility.systemCopyBuffer = request.downloadHandler.text;
                    }
                }
                else
                {
                    error = request.downloadHandler.text;
                }

                if (string.IsNullOrEmpty(error))
                {
                    error = request.error;
                }

                Debug.Log("error in request handler : " + request.error + "\n" + error);
                onError(error);
            }
        }
    }
}