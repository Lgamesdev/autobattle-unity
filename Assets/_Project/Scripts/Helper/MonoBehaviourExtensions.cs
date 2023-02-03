using System.Collections;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace LGamesDev.Helper
{
    public static class MonoBehaviourExtensions
    {
        public static void InvokeWithDelay(this MonoBehaviour mono, Action method, float delay)
        {
            mono.StartCoroutine(CallWithDelayCoroutine(method, delay));
        }
        
        public static void StartCoroutineWithDelay(this MonoBehaviour mono, IEnumerator method, float delay)
        {
            mono.StartCoroutine(CallWithDelayCoroutine(mono, method, delay));
        }

        private static IEnumerator CallWithDelayCoroutine(Action method, float delay)
        {
            Debug.Log("delay : " + delay);
            yield return new WaitForSeconds(delay);
            Debug.Log("method execute");
            method();
        }
        
        private static IEnumerator CallWithDelayCoroutine(MonoBehaviour mono, IEnumerator method, float delay)
        {
            Debug.Log("delay : " + delay);
            yield return new WaitForSeconds(delay);
            Debug.Log("coroutine execute");
            mono.StartCoroutine(method);
        }
    }
}