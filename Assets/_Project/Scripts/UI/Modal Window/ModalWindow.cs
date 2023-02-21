using System;
using UnityEngine;

namespace LGamesDev.UI
{
    public class ModalWindow
    {
        public string Title;
        public Sprite Icon;
        public string Message;
        public string ConfirmMessage;
        public string DeclineMessage;
        public Action ConfirmAction = null;
        public Action DeclineAction = null;
        public Action AlternateAction = null;
        public ModalType ModalType;

    }

    public enum ModalType
    {
        Hero,
        Prompt,
        Dialog,
        TextPopup
    }
}