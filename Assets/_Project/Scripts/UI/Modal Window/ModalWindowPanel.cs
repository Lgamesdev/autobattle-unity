using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindowPanel : MonoBehaviour
{
    [Header("Main Image")] 
    [SerializeField] private Transform imageArea;
    [SerializeField] private Image mainImage;
    [Space()]
    [SerializeField] private Transform box;
    
    [Header("Header")]
    [SerializeField] private Transform headerArea;
    [SerializeField] private TextMeshProUGUI titleField;
    
    [Header("Content")]
    [SerializeField] private Transform contentArea;
    [SerializeField] private Transform verticalLayoutArea;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroText;
    [Space()]
    [SerializeField] private Transform horizontalLayoutArea;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI iconText;

    [Header("Footer")] 
    [SerializeField] private Transform footerArea;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private Button declineButton;
    [SerializeField] private TextMeshProUGUI declineText;
    [SerializeField] private Button alternateButton;

    private Action _onConfirmAction;
    private Action _onDeclineAction;
    private Action _onAlternateAction;

    private readonly List<ModalWindow> _nextModals = new();

    public void ShowAsHero(string title, Sprite imageToShow, string message, Action confirmAction)
    {
        ShowAsHero(title, imageToShow, message, "Continue", "", confirmAction);
    }
    
    public void ShowAsHero(string title, Sprite imageToShow, string message, Action confirmAction, Action declineAction)
    {
        ShowAsHero(title, imageToShow, message, "Continue", "Back", confirmAction, declineAction);
    }

    private void ShowAsHero(string title, Sprite imageToShow, string message, string confirmMessage, string declineMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        if (gameObject == null) return;

        if (gameObject.activeSelf)
        {
            ModalWindow nextModal = new ModalWindow()
            {
                Title = title,
                Icon = imageToShow,
                Message = message,
                ConfirmMessage = confirmMessage,
                DeclineMessage = declineMessage,
                ConfirmAction = confirmAction,
                DeclineAction = declineAction,
                AlternateAction = alternateAction,
                ModalType = ModalType.TextPopup
            };
            
            if(!_nextModals.Contains(nextModal))
                _nextModals.Add(nextModal);
        }
        else
        {
            LeanTween.cancel(gameObject);

            horizontalLayoutArea.gameObject.SetActive(false);
            verticalLayoutArea.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(true);

            //Hide the header if there's no title
            bool hasTitle = !string.IsNullOrEmpty(title);
            headerArea.gameObject.SetActive(hasTitle);
            titleField.text = title;

            heroImage.sprite = imageToShow;
            heroText.text = message;

            _onConfirmAction = confirmAction;
            confirmText.text = confirmMessage;

            bool hasDecline = (declineAction != null);
            declineButton.gameObject.SetActive(hasDecline);
            declineText.text = declineMessage;
            _onDeclineAction = declineAction;

            bool hasAlternate = (alternateAction != null);
            alternateButton.gameObject.SetActive(hasAlternate);
            _onAlternateAction = alternateAction;

            Show();
        }
    }

    public void ShowAsPrompt(string title, Sprite icon, string message)
    {
        ShowAsPrompt(title, icon, message, "Continue", "", null);
    }

    private void ShowAsPrompt(string title, Sprite icon, string message, string confirmMessage, string declineMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        if (gameObject == null) return;

        if (gameObject.activeSelf)
        {
            ModalWindow nextModal = new ModalWindow()
            {
                Title = title,
                Icon = icon,
                Message = message,
                ConfirmMessage = confirmMessage,
                DeclineMessage = declineMessage,
                ConfirmAction = confirmAction,
                DeclineAction = declineAction,
                AlternateAction = alternateAction,
                ModalType = ModalType.TextPopup
            };
            
            if(!_nextModals.Contains(nextModal))
                _nextModals.Add(nextModal);
        }
        else
        {
            LeanTween.cancel(box.gameObject);

            horizontalLayoutArea.gameObject.SetActive(true);
            verticalLayoutArea.gameObject.SetActive(false);

            //Hide the header if there's no title
            bool hasTitle = string.IsNullOrEmpty(title);
            headerArea.gameObject.SetActive(hasTitle);
            titleField.text = title;

            iconImage.sprite = icon;
            iconText.text = message;

            _onConfirmAction = confirmAction;
            confirmText.text = confirmMessage;

            bool hasDecline = (declineAction != null);
            declineButton.gameObject.SetActive(hasDecline);
            declineText.text = declineMessage;
            _onDeclineAction = declineAction;

            bool hasAlternate = (alternateAction != null);
            alternateButton.gameObject.SetActive(hasAlternate);
            _onAlternateAction = alternateAction;

            Show();
        }
    }

    private void ShowAsDialog(string title, string message, string confirmMessage, string declineMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        if (gameObject == null) return;

        if (gameObject.activeSelf)
        {
            ModalWindow nextModal = new ModalWindow()
            {
                Title = title,
                Message = message,
                ConfirmMessage = confirmMessage,
                DeclineMessage = declineMessage,
                ConfirmAction = confirmAction,
                DeclineAction = declineAction,
                AlternateAction = alternateAction,
                ModalType = ModalType.TextPopup
            };
            
            if(!_nextModals.Contains(nextModal))
                _nextModals.Add(nextModal);
        }
        else
        {
            LeanTween.cancel(box.gameObject);

            horizontalLayoutArea.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(false);
            verticalLayoutArea.gameObject.SetActive(false);

            //Hide the header if there's no title
            bool hasTitle = string.IsNullOrEmpty(title);
            headerArea.gameObject.SetActive(hasTitle);
            titleField.text = title;

            iconText.text = message;

            _onConfirmAction = confirmAction;
            confirmText.text = confirmMessage;

            bool hasDecline = (declineAction != null);
            declineButton.gameObject.SetActive(hasDecline);
            declineText.text = declineMessage;
            _onDeclineAction = declineAction;

            bool hasAlternate = (alternateAction != null);
            alternateButton.gameObject.SetActive(hasAlternate);
            _onAlternateAction = alternateAction;

            Show();
        }
    }
    
    public void ShowAsTextPopup(string title, string message, string confirmMessage, string declineMessage, Action confirmAction = null, Action declineAction = null, Action alternateAction = null)
    {
        if (gameObject == null) return;

        if (gameObject.activeSelf)
        {
            ModalWindow nextModal = new ModalWindow()
            {
                Title = title,
                Message = message,
                ConfirmMessage = confirmMessage,
                DeclineMessage = declineMessage,
                ConfirmAction = confirmAction,
                DeclineAction = declineAction,
                AlternateAction = alternateAction,
                ModalType = ModalType.TextPopup
            };
            
            if(!_nextModals.Contains(nextModal))
                _nextModals.Add(nextModal);
        }
        else
        {
            LeanTween.cancel(gameObject);

            horizontalLayoutArea.gameObject.SetActive(false);
            verticalLayoutArea.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);

            //Hide the header if there's no title
            bool hasTitle = !string.IsNullOrEmpty(title);
            headerArea.gameObject.SetActive(hasTitle);
            titleField.text = title;

            heroText.text = message;

            bool hasConfirm = (confirmAction != null);
            confirmButton.gameObject.SetActive(hasConfirm);
            confirmText.text = confirmMessage;
            _onConfirmAction = confirmAction;

            bool hasDecline = (declineAction != null);
            declineButton.gameObject.SetActive(hasDecline);
            declineText.text = declineMessage;
            _onDeclineAction = declineAction;

            bool hasAlternate = (alternateAction != null);
            alternateButton.gameObject.SetActive(hasAlternate);
            _onAlternateAction = alternateAction;

            Show();
        }
    }

    public void Confirm()
    {
        _onConfirmAction?.Invoke();
        Close();
    }
    
    public void Decline()
    {
        _onDeclineAction?.Invoke();
        Close();
    }
    
    public void Alternate()
    {
        _onAlternateAction?.Invoke();
        Close();
    }
    
    private void Show()
    {
        box.gameObject.SetActive(false);
        if (imageArea != null) {
            imageArea.gameObject.SetActive(false);
        }

        Image panelImage = GetComponent<Image>();
        gameObject.SetActive(true);
        LeanTween.value(0f, .25f, .25f).setOnUpdate(
        (float val) =>
        {
            Color c = panelImage.color;
            c.a = val;
            panelImage.color = c;
        }).setEase(LeanTweenType.linear).setOnComplete(ShowModalBox);
    }

    private void ShowModalBox()
    {
        box.gameObject.SetActive(true);
        if (imageArea != null)
        {
            LeanTween.scale(imageArea.gameObject, new Vector3(1f, 1f, 1f), .25f).setFrom(new Vector3(.2f, .2f, .2f))
                .setEase(LeanTweenType.linear).setOnComplete(ShowMainImage);
        }
    }

    private void ShowMainImage()
    {
        imageArea.gameObject.SetActive(true);
        LeanTween.scale(box.gameObject, new Vector3(1f, 1f, 1f), .25f).setFrom(new Vector3(.2f, .2f, .2f)).setEase(LeanTweenType.linear);
    }

    public void Close()
    {
        LeanTween.cancel(gameObject);
        gameObject.SetActive(false);

        if (_nextModals.Count > 0)
        {
            ModalWindow nextModal = _nextModals.First();
            switch (nextModal.ModalType)
            {
                case ModalType.Hero:
                    ShowAsHero(
                        nextModal.Title, 
                        nextModal.Icon,
                        nextModal.Message,
                        nextModal.ConfirmMessage,
                        nextModal.DeclineMessage,
                        nextModal.ConfirmAction,
                        nextModal.DeclineAction,
                        nextModal.AlternateAction);
                    break;
                case ModalType.Prompt:
                    ShowAsPrompt(
                        nextModal.Title, 
                        nextModal.Icon,
                        nextModal.Message,
                        nextModal.ConfirmMessage,
                        nextModal.DeclineMessage,
                        nextModal.ConfirmAction,
                        nextModal.DeclineAction,
                        nextModal.AlternateAction);
                    break;
                case ModalType.Dialog:
                    ShowAsDialog(
                        nextModal.Title, 
                        nextModal.Message,
                        nextModal.ConfirmMessage,
                        nextModal.DeclineMessage,
                        nextModal.ConfirmAction,
                        nextModal.DeclineAction,
                        nextModal.AlternateAction);
                    break;
                case ModalType.TextPopup:
                    ShowAsTextPopup(
                        nextModal.Title, 
                        nextModal.Message,
                        nextModal.ConfirmMessage,
                        nextModal.DeclineMessage,
                        nextModal.ConfirmAction,
                        nextModal.DeclineAction,
                        nextModal.AlternateAction);
                    break;
            }
            _nextModals.Remove(nextModal);
        }
    }
}
