using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerDownHandler
{
    public TabGroup tabGroup;

    public Image background;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
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
