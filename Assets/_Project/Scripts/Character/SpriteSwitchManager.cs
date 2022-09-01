using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LGamesDev
{
    public class SpriteSwitchManager : MonoBehaviour
    {
        public List<SpriteSwitcher> spriteSwitchers;
        public List<ColorPickerButton> colorPickers;

        private void Start()
        {
            SpriteLibManager.Instance.OnSpriteLibChange += ResetSprites;
        }

        private void ResetSprites(SpriteLib spriteLib)
        {
            foreach (SpriteSwitcher switcher in spriteSwitchers)
            {
                switcher.Reset();
            }

            foreach (ColorPickerButton colorPickerButton in colorPickers)
            {
                colorPickerButton.GetComponent<Image>().color = colorPickerButton.activeColor;
            }
        }

        public void RandomizeSprites()
        {
            foreach (SpriteSwitcher switcher in spriteSwitchers)
            {
                switcher.Randomize();
            }

            foreach (ColorPickerButton colorPickerButton in colorPickers)
            {
                colorPickerButton.Randomize();
            }
        }
    }

    public enum SwitchBodyPart
    {
        Hair,
        Beard,
        Moustache,
    }

    public enum SwitchBodyColorPart
    {
        Hair,
        Skin,
        Chest,
        Belt,
        Short
    }
}
