using System.Collections;
using LGamesDev.Core.Request;
using LGamesDev.Fighting;
using LGamesDev.UI;
using UnityEngine;
using UnityEngine.Networking;
using FightHandler = LGamesDev.Core.Request.FightHandler;

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

        private void SetupScene()
        {
            if (!_gameManager.GetPlayerConfig().tutorialDone)
                firstPopup.PopUp("Kalcifer : Salut et bienvenue dans mon tout premier jeu ! :)");
        }

        public void TutorialDone()
        {
            StartCoroutine(RequestHandler.Request("api/user/tutorialDone",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /tutorialDone : " + error); },
                response => { Debug.Log("Received /tutorialDone : " + response); })
            );
        }

        public void LoadPvpFight()
        {
            StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    Debug.Log("fight request result : " + result.ToString());
                    
                    StartCoroutine(_gameManager.LoadFight(result));
                }
            ));
            
        }
        
        public void LoadPveFight()
        {
            StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    Debug.Log("fight request result : " + result.ToString());

                    StartCoroutine(_gameManager.LoadFight(result));
                }
            ));
        }
    }
}