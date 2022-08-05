using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev.Core.Request
{
    public class RequestHandler : MonoBehaviour
    {
        private const string BaseURL = @"http://autobattle.hopto.org:35080/";

        public static IEnumerator Request(string url, string httpVerb, Action<string> onError, Action<string> onSuccess,
            byte[] bodyRaw = null,
            Authentication.Authentication authentication = null)
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
                //Debug.Log("error : " + request.downloadHandler.text);
                onError(request.error);
            }
        }
    }
}