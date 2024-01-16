using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class MainMenuCleanUp : MonoBehaviour
{
    private void Awake() {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GameMultiplayer.Instance != null) {
            Destroy(GameMultiplayer.Instance.gameObject);
        }

        if (GameLobby.Instance != null) {
            Destroy(GameLobby.Instance.gameObject);
        }
    }
}
