using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public GameObject objectToSwap;

    public Image background;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;


    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //tabGroup.OnTabEnter(this);
        background.color = new Color(
            background.color.r - 0.10f, 
            background.color.g - 0.10f,
            background.color.b - 0.10f
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tabGroup.OnTabExit(this);
        background.color = new Color(
            background.color.r + 0.10f, 
            background.color.g + 0.10f,
            background.color.b + 0.10f
        );
    }

    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke();
    }
}
