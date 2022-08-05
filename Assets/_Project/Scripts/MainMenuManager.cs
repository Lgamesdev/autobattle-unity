using LGamesDev.Core.Request;
using LGamesDev.UI;
using UnityEngine;
using UnityEngine.Networking;

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
    }
}