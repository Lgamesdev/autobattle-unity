using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.UI
{
    public class AuthenticationUI : MonoBehaviour
    {
        [SerializeField] private Transform mainPanel;
        [SerializeField] private Transform platformRegisterForm;
        [SerializeField] private Transform registerForm;
        [SerializeField] private Transform loginForm;

        private Transform _activeForm;

        private void Start()
        {
            platformRegisterForm.gameObject.SetActive(false);
            registerForm.gameObject.SetActive(false);
            loginForm.gameObject.SetActive(false);

            AuthenticationManager.Instance.OnStateUpdate += UpdateUI;
        }

        private void UpdateUI(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.Default:
                    _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text = "";
                    if (_activeForm == registerForm)
                    {
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text = "";
                        _activeForm.Find("rePasswordField").GetComponent<TMP_InputField>().text = "";
                    }
                    if (_activeForm == (registerForm || loginForm))
                    {
                        _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text = "";
                    }

                    _activeForm.gameObject.SetActive(false);
                    mainPanel.gameObject.SetActive(true);
                    break;
                case AuthenticationState.PlatformRegister:
                    mainPanel.gameObject.SetActive(false);
                    platformRegisterForm.gameObject.SetActive(true);

                    _activeForm = platformRegisterForm;
                    break;
                case AuthenticationState.Register:
                    mainPanel.gameObject.SetActive(false);
                    registerForm.gameObject.SetActive(true);

                    _activeForm = registerForm;
                    break;
                case AuthenticationState.Login:
                    mainPanel.gameObject.SetActive(false);
                    loginForm.gameObject.SetActive(true);

                    _activeForm = loginForm;
                    break;
                case AuthenticationState.Refresh:
                    //Todo: Waiting screen 
                    break;
            }
        }

        public void Submit()
        {
            if (_activeForm == registerForm)
            {
                if (!_activeForm.Find("passwordField").GetComponent<TMP_InputField>().text
                        .Equals(_activeForm.Find("rePasswordField").GetComponent<TMP_InputField>().text)) 
                {
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error on register", 
                        "Password must correspond to the password verification.", 
                        "", 
                        "Close",
                        null,
                        GameManager.Instance.modalWindow.Close
                    );
                } 
                else 
                {
                    AuthenticationManager.Instance.Submit(
                        _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text
                    );
                }
            }
            else
            {
                AuthenticationManager.Instance.Submit(
                    _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                    _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text
                );
            }
        }

        private static void ShowLoginError(string message)
        {
            //_loadingPanel.gameObject.SetActive(false);
            //_errorText.gameObject.SetActive(true);
            //_errorText.text = message;
        }

        public void VerifyInputs()
        {
            //Todo
        }
    }
}