using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using LGamesDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpriteColorPicker : MonoBehaviour/*, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler*/
{
    public static SpriteColorPicker Instance;

    public SwitchBodyPart switchBodyPart;

    [SerializeField] private GameObject pfColorButton;
    public ColorPickerButton currentColorPicker;
    
    private bool _mouseOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(ColorPickerButton colorPickerButton)
    {
        currentColorPicker = colorPickerButton;

        foreach (Transform child in transform.Find("ColorMask").Find("Colors")) Destroy(child.gameObject);

        foreach (Color color in currentColorPicker.colorLibrary.colors)
        {
            ColorButton colorButton = Instantiate(pfColorButton, transform.Find("ColorMask").Find("Colors")).GetComponent<ColorButton>();
            colorButton.SetColor(color);
        }
        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /*private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!_mouseOver) gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
            return;
        _mouseOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }*/
}
