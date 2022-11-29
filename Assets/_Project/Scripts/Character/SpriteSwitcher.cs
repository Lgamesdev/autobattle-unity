using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class SpriteSwitcher : MonoBehaviour
{
    public SpriteResolver bodyPart;

    public SwitchBodyPart switchBodyPart;

    public int activeIndex;

    private void OnEnable()
    {
        InitializeSprite();
    }

    private void InitializeSprite()
    {
        bodyPart = null;

        bodyPart = switchBodyPart switch
        {
            SwitchBodyPart.Hair => CharacterManager.Instance.activeCharacter.hairResolver,
            SwitchBodyPart.Moustache => CharacterManager.Instance.activeCharacter.moustacheResolver,
            SwitchBodyPart.Beard => CharacterManager.Instance.activeCharacter.beardResolver,
            _ => bodyPart
        };

        Reset();
    }

    public void NextOption()
    {
        List<string> labels = bodyPart.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(bodyPart.GetCategory()).ToList();

        activeIndex = labels.FindIndex(a => a.Contains(bodyPart.GetLabel())) + 1;
        if (activeIndex >= labels.Count - 1)
        {
            activeIndex = 0;
        }

        bodyPart.SetCategoryAndLabel(bodyPart.GetCategory(), labels[activeIndex]);
    }
    
    public void PreviousOption()
    {
        List<string> labels = bodyPart.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(bodyPart.GetCategory()).ToList();

        activeIndex = labels.FindIndex(a => a.Contains(bodyPart.GetLabel())) - 1;
        if (activeIndex <= -1)
        {
            activeIndex = labels.Count - 1;
        }

        bodyPart.SetCategoryAndLabel(bodyPart.GetCategory(), labels[activeIndex]);
    }

    public void Reset()
    {
        List<string> labels = bodyPart.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(bodyPart.GetCategory()).ToList();
        activeIndex = 0;

        bodyPart.SetCategoryAndLabel(bodyPart.GetCategory(), labels[activeIndex]);
    }

    public void Randomize()
    {
        List<string> labels = bodyPart.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(bodyPart.GetCategory()).ToList();
        activeIndex = Random.Range(0, labels.Count);

        bodyPart.SetCategoryAndLabel(bodyPart.GetCategory(), labels[activeIndex]);
    }
}
