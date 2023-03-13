using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace LGamesDev
{
    public class DialogManager : MonoBehaviour, IPointerDownHandler
    {
        public static DialogManager Instance;

        public Image actorImage;
        public TextMeshProUGUI actorName;
        public TextMeshProUGUI messageText;
        public RectTransform backgroundBox;
        public Image backgroundScreen;

        private DialogMessage[] _currentMessages;
        private Actor[] _currentActors;
        private int _activeMessage = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            backgroundBox.transform.localScale = Vector3.zero;
            backgroundScreen.color =
                new Color(backgroundScreen.color.r, backgroundScreen.color.g, backgroundScreen.color.b, 0);
            backgroundScreen.raycastTarget = false;
        }

        public void OpenDialog(DialogMessage[] messages, Actor[] actors)
        {
            _currentMessages = messages;
            _currentActors = actors;
            _activeMessage = 0;

            DisplayMessage();
            backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
            LeanTween.value(gameObject, 0, 0.2f, .5f).setOnUpdate(val =>
            {
                Color c = backgroundScreen.color;
                c.a = val;
                backgroundScreen.color = c;
            });
            backgroundScreen.raycastTarget = true;
        }

        private void DisplayMessage()
        {
            DialogMessage messageToDisplay = _currentMessages[_activeMessage];
            messageText.text = messageToDisplay.message;

            Actor actorToDisplay = _currentActors[messageToDisplay.actorId];
            actorName.text = actorToDisplay.name;
            if (actorToDisplay.sprite != null)
            {
                actorImage.gameObject.SetActive(true);
                actorImage.sprite = actorToDisplay.sprite;
            }
            else
            {
                actorImage.gameObject.SetActive(false);
            }

            AnimateTextColor();
            
            messageToDisplay.action?.Invoke();
        }

        private void NextMessage()
        {
            _activeMessage++;
            if (_activeMessage < _currentMessages.Length)
            {
                DisplayMessage();
            }
            else
            {
                backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
                LeanTween.value(gameObject, 0.2f, 0, .5f).setOnUpdate(val =>
                {
                    Color c = backgroundScreen.color;
                    c.a = val;
                    backgroundScreen.color = c;
                });
                backgroundScreen.raycastTarget = false;
            }
        }

        private void AnimateTextColor()
        {
            Color color = messageText.color;
            Color fadeOutColor = color;
            fadeOutColor.a = 0;
            LeanTween.value(gameObject, val =>
            {
                messageText.color = val;
            }, fadeOutColor, color, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            NextMessage();
        }
    }

}