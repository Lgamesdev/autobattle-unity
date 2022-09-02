using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class SpriteSwitcher : MonoBehaviour
{
    [Header("SpriteResolvers To Switch")]
    public List<SpriteResolver> bodyParts;

    public SwitchBodyPart switchBodyPart;

    public int activeIndex;

    private void Start()
    {
        Reset();
    }

    public void NextOption()
    {
        foreach (SpriteResolver sr in bodyParts)
        {
            List<string> labels = sr.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(sr.GetCategory()).ToList();

            activeIndex = labels.FindIndex(a => a.Contains(sr.GetLabel())) + 1;
            if (activeIndex >= labels.Count - 1)
            {
                activeIndex = 0;
            }

            sr.SetCategoryAndLabel(sr.GetCategory(), labels[activeIndex]);
        }
    }
    
    public void PreviousOption()
    {
        foreach (SpriteResolver sr in bodyParts)
        {
            List<string> labels = sr.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(sr.GetCategory()).ToList();

            activeIndex = labels.FindIndex(a => a.Contains(sr.GetLabel())) - 1;
            if (activeIndex <= -1)
            {
                activeIndex = labels.Count - 1;
            }

            sr.SetCategoryAndLabel(sr.GetCategory(), labels[activeIndex]);
        }
    }

    public void Reset()
    {
        foreach (SpriteResolver sr in bodyParts)
        {
            List<string> labels = sr.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(sr.GetCategory()).ToList();
            activeIndex = 1;

            sr.SetCategoryAndLabel(sr.GetCategory(), labels[activeIndex]);
        }
    }

    public void Randomize()
    {
        foreach (SpriteResolver sr in bodyParts)
        {
            List<string> labels = sr.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(sr.GetCategory()).ToList();
            activeIndex = Random.Range(0, labels.Count);

            sr.SetCategoryAndLabel(sr.GetCategory(), labels[activeIndex]);
        }
    }
}
