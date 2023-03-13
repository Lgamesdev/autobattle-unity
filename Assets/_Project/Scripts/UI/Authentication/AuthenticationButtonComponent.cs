using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;
using UnityEngine.Serialization;

public class AuthenticationButtonComponent : MonoBehaviour
{
    public AuthenticationState authenticationState;

    public void OnClick()
    {
        AuthenticationManager.Instance.SetState(authenticationState);
    }
}
