using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColorPickerButton : MonoBehaviour, IPointerDownHandler
{
    public ColorLibrary colorLibrary;
    public List<SpriteRenderer> spriteRenderers;
    
    public SwitchBodyColorPart switchBodyColorPart;

    public Color activeColor;

    private void OnEnable()
    {
        InitializeSpriteColor();
    }

    private void InitializeSpriteColor()
    {
        spriteRenderers.Clear();
        
        switch (switchBodyColorPart)
        {
            case SwitchBodyColorPart.Hair:
                spriteRenderers.Add(CharacterManager.Instance.activeCharacter.hairResolver.GetComponent<SpriteRenderer>());
                spriteRenderers.Add(CharacterManager.Instance.activeCharacter.eyeBrowsRenderer);
                if (CharacterManager.Instance.activeSpriteLib == SpriteLib.Male)
                {
                    spriteRenderers.Add(CharacterManager.Instance.activeCharacter.moustacheResolver
                        .GetComponent<SpriteRenderer>());
                    spriteRenderers.Add(CharacterManager.Instance.activeCharacter.beardResolver
                        .GetComponent<SpriteRenderer>());
                }
                break;
            
            case SwitchBodyColorPart.Skin:
                foreach (SpriteResolver resolver in CharacterManager.Instance.activeCharacter.bodyResolvers)
                {
                    spriteRenderers.Add(resolver.GetComponent<SpriteRenderer>());
                }
                break;
            
            case SwitchBodyColorPart.Chest:
                foreach (SpriteResolver resolver in CharacterManager.Instance.activeCharacter.chestResolvers)
                {
                    spriteRenderers.Add(resolver.GetComponent<SpriteRenderer>());
                }
                break;
            
            case SwitchBodyColorPart.Pants:
                foreach (SpriteResolver resolver in CharacterManager.Instance.activeCharacter.pantResolvers)
                {
                    spriteRenderers.Add(resolver.GetComponent<SpriteRenderer>());
                }
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

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
