using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;
using UnityEngine.Events;

public class HeroWindowTrigger : MonoBehaviour
{
    public string title;
    public Sprite sprite;
    public string message;
    public bool triggerOnEnable = true;

    public UnityEvent onContinueCallback;
    public UnityEvent onCancelCallback;
    public UnityEvent onAlternateCallback;
    
    public void OnEnable()
    {
        if (!triggerOnEnable)
        {
            return;
        }

        Action continueCallback = null;
        Action cancelCallback = null;
        //Action alternateCallback = null;

        if (onContinueCallback.GetPersistentEventCount() > 0)
        {
            continueCallback = onContinueCallback.Invoke;
        }
        if (onCancelCallback.GetPersistentEventCount() > 0)
        {
            cancelCallback = onCancelCallback.Invoke;
        }
        /*if (onAlternateCallback.GetPersistentEventCount() > 0)
        {
            alternateCallback = onAlternateCallback.Invoke;
        }*/

        GameManager.Instance.modalWindow.ShowAsHero(title, sprite, message, continueCallback, cancelCallback);
    }
}
