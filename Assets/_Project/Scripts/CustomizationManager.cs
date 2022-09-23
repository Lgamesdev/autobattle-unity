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
            Body form = new Body {beardIndex = 0, moustacheIndex = 0, chestColor = "#dc0505"};

            SpriteLibSwitchComponent spriteComp = tabGroup.selectedTab.GetComponent<SpriteLibSwitchComponent>();
            SpriteSwitchManager spriteManager = tabGroup.objectActive.GetComponent<SpriteSwitchManager>();

            form.isMaleGender = spriteComp.spriteLib == SpriteLib.Male;
            
            foreach (SpriteSwitcher spriteSwitcher in spriteManager.spriteSwitchers)
            {
                switch (spriteSwitcher.switchBodyPart)
                {
                    case SwitchBodyPart.Hair:
                        form.hairIndex = spriteSwitcher.activeIndex;
                        break;
                    case SwitchBodyPart.Moustache:
                        form.moustacheIndex = spriteSwitcher.activeIndex;
                        break;
                    case SwitchBodyPart.Beard:
                        form.beardIndex = spriteSwitcher.activeIndex;
                        break;
                }
            }
            foreach (ColorPickerButton colorPicker in spriteManager.colorPickers)
            {
                switch (colorPicker.switchBodyColorPart)
                {
                    case SwitchBodyColorPart.Hair:
                        form.hairColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Skin:
                        form.skinColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Chest:
                        form.chestColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Belt:
                        form.beltColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                    case SwitchBodyColorPart.Short:
                        form.shortColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                }
            }
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(form));

            StartCoroutine(RequestHandler.Request("api/user/body",
                UnityWebRequest.kHttpVerbPUT,
                error =>
                {
                    Debug.Log("Error : " + error);
                    //OnAuthenticationError?.Invoke(error);

                    StartCoroutine(_gameManager.DisableLoadingScreen());
                },
                response =>
                {
                    Debug.Log("Received : " + response);
                    
                    _gameManager.LoadGame();
                },
                bodyRaw,
                _gameManager.GetAuthentication())
            );
        }
    }
}
