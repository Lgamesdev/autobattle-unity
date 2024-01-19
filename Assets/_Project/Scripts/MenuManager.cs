using System.Collections;
using System.Collections.Generic;
using Core.Player;
using LGamesDev;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    public static PlayerConfig PlayerConfig;
    
    private void Awake()
    {
        Instance = this;

        if (PlayerConfig == null)
        {
            Loader.Load(Loader.Scene.AuthenticationScene);
        }
    }
    
    private void Start()
    {
        if (!PlayerConfig.CreationDone)
        {
            //_gameManager.LoadCustomization();
            Loader.Load(Loader.Scene.CustomizationScene);
        }

        Initialisation.Current.LoadMainMenu();
        //_gameManager.PlayMainMenuMusic();
    }
}
