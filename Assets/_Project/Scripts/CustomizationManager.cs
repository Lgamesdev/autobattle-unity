using System;
using System.Text;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.Networking;

namespace LGamesDev
{
    public class CustomizationManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public TabGroup tabGroup;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        public void SubmitUserBody()
        {
            Body body = new Body {beardIndex = 0, moustacheIndex = 0, chestColor = "#dc0505"};

            SpriteLibSwitchComponent spriteComp = tabGroup.selectedTab.GetComponent<SpriteLibSwitchComponent>();
            SpriteSwitchManager spriteManager = tabGroup.objectActive.GetComponent<SpriteSwitchManager>();

            body.isMaleGender = spriteComp.spriteLib == SpriteLib.Male;
            
            foreach (SpriteSwitcher spriteSwitcher in spriteManager.spriteSwitchers)
            {
                switch (spriteSwitcher.switchBodyPart)
                {
                    case SwitchBodyPart.Hair:
                        body.hairIndex = spriteSwitcher.activeIndex;
                        break;
                    case SwitchBodyPart.Moustache:
                        body.moustacheIndex = spriteSwitcher.activeIndex;
                        break;
                    case SwitchBodyPart.Beard:
                        body.beardIndex = spriteSwitcher.activeIndex;
                        break;
                }
            }
            foreach (ColorPickerButton colorPicker in spriteManager.colorPickers)
            {
                switch (colorPicker.switchBodyColorPart)
                {
                    case SwitchBodyColorPart.Hair:
                        body.hairColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Skin:
                        body.skinColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Chest:
                        body.chestColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Belt:
                        body.beltColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Short:
                        body.shortColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                }
            }
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(body));

            StartCoroutine(CharacterBodyHandler.Save(
                this,
                body,
                error =>
                {
                    Debug.Log("Error : " + error);
                },
                response =>
                {
                    Debug.Log("Received : " + response);
                    
                    _gameManager.LoadMainMenu();
                }
            ));
        }
    }
}
