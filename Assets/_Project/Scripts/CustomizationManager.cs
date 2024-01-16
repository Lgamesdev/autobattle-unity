using System;
using Core.Player;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class CustomizationManager : MonoBehaviour
    {
        public static CustomizationManager Instance;
        
        private GameManager _gameManager;

        public TabGroup tabGroup;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            
            if (_gameManager == null)
            {
                //SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
                Loader.Load(Loader.Scene.LoadingScene);
            }
        }

        public void HandleTutorial()
        {
            GetComponent<DialogTrigger>().StartDialog();
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
                    case SwitchBodyColorPart.Pants:
                        body.shortColor = "#" + ColorUtility.ToHtmlStringRGB(colorPicker.activeColor);
                        break;
                }
            }

            CharacterBodyHandler.Save(
                body,
                error =>
                {
                    Debug.Log("Error : " + error);
                },
                response =>
                {
                    //Debug.Log("Received : " + response);

                    PlayerConfig playerConfig = _gameManager.GetPlayerConf();
                    playerConfig.CreationDone = true;
                    _gameManager.SetPlayerConf(playerConfig);
                    
                    Loader.Load(Loader.Scene.MenuScene);
                }
            );
        }
    }
}
