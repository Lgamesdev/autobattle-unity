using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LGamesDev
{
    public class DialogTrigger : MonoBehaviour
    {
        public DialogMessage[] messages;
        public Actor[] actors;
        
        public void StartDialog()
        {
            DialogManager.Instance.OpenDialog(messages, actors);
        }
    }

    [Serializable]
    public class DialogMessage
    {
        public int actorId;
        public string message;
        public UnityEvent action;
    }

    [Serializable]
    public class Actor
    {
        public string name;
        public Sprite sprite;
    }
}
