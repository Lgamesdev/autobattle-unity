using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public void Logout()
    {
        Authentication authentication = GameManager.Instance.GetAuthentication();

        StartCoroutine(AuthenticationHandler.Logout(
            this,
            authentication.refresh_token,
            () => {
                GameManager.Instance.Logout();
            }
        ));
    }
}
