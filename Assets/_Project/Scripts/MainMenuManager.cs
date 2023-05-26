using System;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using LGamesDev.Fighting;
using LGamesDev.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;
        
        public PopupHandler firstPopup;

        private GameManager _gameManager;

        private void Awake()
        {
            Instance = this;
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
                    _gameManager.PlayMainMenuMusic();
                }
            }
        }

        public void HandleTutorial()
        {
            if (!_gameManager.GetAuthentication().PlayerConf.TutorialDone) {
                GetComponent<DialogTrigger>().StartDialog();
            }
        }

        public void TutorialFinished()
        {
            _gameManager.networkManager.TutorialFinished();
            /*StartCoroutine(RequestHandler.Request("api/user/tutorialDone",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /tutorialDone : " + error); },
                response => { Debug.Log("Received /tutorialDone : " + response); },
                null,
                _gameManager.GetAuthentication())
            );*/
        }

        public void LoadPvpFight()
        {
            _gameManager.networkManager.SearchFight(FightType.Pvp);
            /*StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _gameManager.LoadFight(result);
                }
            ));*/
            
        }
        
        public void LoadPveFight()
        {
            _gameManager.networkManager.SearchFight(FightType.Pve);
            /*StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _gameManager.LoadFight(result);
                }
            ));*/
        }
    }
}