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
        Pants
    }
}
