using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class AuthenticationUI : MonoBehaviour
    {
        [SerializeField] private Transform mainPanel;
        [SerializeField] private Button googleSignInButton;
        [SerializeField] private Button appleSignInButton;
        
        [SerializeField] private Transform platformRegisterForm;
        [SerializeField] private Transform registerForm;
        [SerializeField] private Transform loginForm;
        
        private Transform _activeForm;

        private void Start()
        {
            platformRegisterForm.gameObject.SetActive(false);
            registerForm.gameObject.SetActive(false);
            loginForm.gameObject.SetActive(false);

            #if UNITY_ANDROID
                googleSignInButton.gameObject.SetActive(true);
            #else
                googleSignInButton.gameObject.SetActive(false);
            #endif
            
            #if UNITY_IOS
                appleSignInButton.gameObject.SetActive(true);
            #else
                appleSignInButton.gameObject.SetActive(false);
            #endif
            
            _activeForm = mainPanel;
            AuthenticationManager.Instance.OnStateUpdate += UpdateUI;
        }

        private void UpdateUI(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.Loading 
                    or AuthenticationState.Connect:
                        StartCoroutine(GameManager.Instance.loadingScreen.EnableWaitingScreen());
                        break;
                
                case AuthenticationState.Default:
                    if (_activeForm != mainPanel)
                    {                   
                        _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text = "";
                    }
                    if (_activeForm == registerForm || _activeForm == loginForm)
                    {
                        _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text = "";
                    }
                    if (_activeForm == registerForm)
                    {
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text = "";
                        _activeForm.Find("rePasswordField").GetComponent<TMP_InputField>().text = "";
                    }
                    if (_activeForm != null)
                    {
                        _activeForm.gameObject.SetActive(false);
                        mainPanel.gameObject.SetActive(true);
                    }
                    break;
                
                case AuthenticationState.Register:
                    mainPanel.gameObject.SetActive(false);
                    platformRegisterForm.gameObject.SetActive(true);
                    _activeForm = platformRegisterForm;
                    break;
            }
            
            if (state != AuthenticationState.Loading 
                && state != AuthenticationState.Connect)
            {
                //Debug.Log("state = " + state + " waiting screen disabled");
                StartCoroutine(GameManager.Instance.loadingScreen.DisableWaitingScreen());
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
                    /*AuthenticationManager.Instance.Submit(
                        _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text
                    );*/
                }
            }
            else if (_activeForm == loginForm)
            {
                /*AuthenticationManager.Instance.Submit(
                    _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                    _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text
                );*/
            } 
            else if (_activeForm == platformRegisterForm)
            {
                if (!string.IsNullOrEmpty(_activeForm.Find("usernameField").GetComponent<TMP_InputField>().text))
                {
                    AuthenticationManager.Instance.Submit(
                        _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text
                    );
                }
                else
                {
                    GameManager.Instance.modalWindow.ShowAsTextPopup(
                        "Error on register",
                        "You must enter a username!", 
                        "", 
                        "Close",
                        null,
                        GameManager.Instance.modalWindow.Close
                    );
                }
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