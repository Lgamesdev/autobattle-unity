using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button findGameButton;

    private void Awake()
    {
        findGameButton.onClick.AddListener(() =>
        {
            GameMultiplayer.Character = CharacterManager.Instance.Character;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        
        Time.timeScale = 1f;
    }
}
