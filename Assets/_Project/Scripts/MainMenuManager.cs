using System;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using LGamesDev.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class MainMenuManager : MonoBehaviour
    {
        public PopupHandler firstPopup;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            //In start method cause of loading scene
            if (_gameManager == null)
            {
                SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            }
            else
            {
                if (!_gameManager.GetAuthentication().PlayerConf.CreationDone)
                {
                    _gameManager.LoadCustomization();
                }
                else
                {
                    Initialisation.Current.LoadMainMenu();
                }
                
                //_gameManager.PlayMainMenuMusic();
            }
        }

        private void SetupScene()
        {
            if (!_gameManager.GetAuthentication().PlayerConf.TutorialDone)
                firstPopup.PopUp("Kalcifer : Salut et bienvenue dans mon tout premier jeu ! :)");
        }

        public void TutorialDone()
        {
            StartCoroutine(RequestHandler.Request("api/user/tutorialDone",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /tutorialDone : " + error); },
                response => { Debug.Log("Received /tutorialDone : " + response); },
                null,
                _gameManager.GetAuthentication())
            );
        }

        public void LoadPvpFight()
        {
            StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _gameManager.LoadFight(result);
                }
            ));
            
        }
        
        public void LoadPveFight()
        {
            StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _gameManager.LoadFight(result);
                }
            ));
        }
    }
}