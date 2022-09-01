using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainWindowPanel : MonoBehaviour
{
    [SerializeField] private Transform box;
    
    [Header("Header")]
    [SerializeField] private Transform headerArea;
    [SerializeField] private TextMeshProUGUI titleField;
    
    [Header("Content")]
    [SerializeField] private Transform contentArea;
    [SerializeField] private Transform inventoryTab;
    [SerializeField] private Transform shopTab;
    [SerializeField] private Transform scoreTab;

    [Header("Footer")] 
    [SerializeField] private Transform footerArea;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button scoreButton;
    [SerializeField] private Button closeButton;

    public void Show()
    {
        LeanTween.cancel(gameObject);

        box.gameObject.SetActive(false);
        //imageArea.gameObject.SetActive(false);

        Image panelImage = GetComponent<Image>();
        gameObject.SetActive(true);
        LeanTween.value(0f, .25f, .5f).setOnUpdate(
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
        LeanTween.scale(box.gameObject, new Vector3(1f, 1f, 1f), .5f).setFrom(new Vector3(.2f, .2f, .2f))
            .setEase(LeanTweenType.linear);
    }

    public void Close()
    {
        LeanTween.cancel(gameObject);
        gameObject.SetActive(false);
    }
}
