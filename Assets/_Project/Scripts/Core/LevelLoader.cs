using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev.Core
{
    public class LevelLoader : MonoBehaviour
    {
        private static Transform _instance;

        public Animator transition;

        public float transitionTime = 1f;

        private void Awake()
        {
            _instance = transform;
        }

        public void LoadScene(SceneIndexes sceneIndex)
        {
            if (_instance == null) Instantiate(GameAssets.i.levelLoader); //à voir

            var levelLoader = _instance.GetComponent<LevelLoader>();

            levelLoader.StartCoroutine(levelLoader.LoadLevel((int)sceneIndex)); // SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadNextLevel(SceneIndexes sceneIndex)
        {
            if (_instance == null) Instantiate(GameAssets.i.levelLoader); //à voir

            var levelLoader = _instance.GetComponent<LevelLoader>();

            levelLoader.StartCoroutine(levelLoader.LoadLevel((int)sceneIndex)); // SceneManager.GetActiveScene().buildIndex;
        }

        private IEnumerator LoadLevel(int sceneIndex)
        {
            //Play animation
            transition.SetTrigger("Start");


            //Wait
            yield return new WaitForSeconds(transitionTime);

            //Load Scene
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public enum SceneIndexes
    {
        PersistentScene = 0,
        Authentication = 1,
        Customization = 2,
        MainMenu = 3,
        Inventory = 4,
        Shop = 5,
        Battle = 6
    }
}