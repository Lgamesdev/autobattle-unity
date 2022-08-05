using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColorPickerButton : MonoBehaviour, IPointerDownHandler
{
    public ColorLibrary colorLibrary;
    public List<SpriteRenderer> spriteRenderers;
    
    public SwitchBodyColorPart switchBodyColorPart;

    public Color activeColor;

    private void Start()
    {
        activeColor = colorLibrary.colors[0];
        SetSpritesColor(activeColor); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SpriteColorPicker.Instance.Show(this);
    }

    public void SetSpritesColor(Color color)
    {
        activeColor = color;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = activeColor;
        }
        
        GetComponent<Image>().color = activeColor;
    }

    public void Randomize()
    {
        SetSpritesColor(colorLibrary.colors[Random.Range(0, colorLibrary.colors.Count)]);
    }
}
