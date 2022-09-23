using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class RequestHandler : MonoBehaviour
    {
        private const string BaseURL = @"http://autobattle.hopto.org:35080/";
        
        public delegate void OnRequestErrorEvent(string error);
        public static OnRequestErrorEvent OnRequestError;

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
                onSuccess(request.downloadHandler.text);
            else {
                Debug.Log("error : " + request.downloadHandler.text);
                var parsedJObjet = JObject.Parse(request.downloadHandler.text);
                OnRequestError?.Invoke(parsedJObjet["detail"]?.ToString());
                onError(request.error);
            }
        }
    }
}