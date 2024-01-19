using System;
using Core.Network;
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

        public ChatService chatService;
        
        public PopupHandler firstPopup;

        private StartManager _startManager;

        private void Awake()
        {
            Instance = this;
            //_startManager = StartManager.Instance;
        }

        private void Start()
        {
            //In start method cause of loading scene
            /*if (_startManager == null)
            {
                SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            }
            else
            {
                
            }*/
            
            /*if (!_startManager.GetPlayerConf().CreationDone)
            {
                //_startManager.LoadCustomization();
                Loader.Load(Loader.Scene.CustomizationScene);
            }
            else
            {
                
            }*/
            Initialisation.Current.LoadMainMenu();
            _startManager.PlayMainMenuMusic();
        }

        public void HandleTutorial()
        {
            if (!_startManager.GetPlayerConf().TutorialDone) {
                GetComponent<DialogTrigger>().StartDialog();
            }
        }

        public void TutorialFinished()
        {
            TutorialHandler.TutorialFinished(e =>
            {
                Debug.Log("error on initialisation : " + e);
            });
            //_startManager.networkService.OnTutorialFinished();
            /*StartCoroutine(RequestHandler.Request("api/user/tutorialDone",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /tutorialDone : " + error); },
                response => { Debug.Log("Received /tutorialDone : " + response); },
                null,
                _startManager.GetAuthentication())
            );*/
        }

        public void LoadPvpFight()
        {
           // _startManager.networkService.SearchFight(FightType.Pvp);
            /*StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _startManager.LoadFight(result);
                }
            ));*/
            
        }
        
        public void LoadPveFight()
        {
            //_startManager.networkService.SearchFight(FightType.Pve);
            /*StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    //Debug.Log("fight request result : " + result.ToString());

                    _startManager.LoadFight(result);
                }
            ));*/
        }
    }
}