using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class AuthenticationUI : MonoBehaviour
    {
        private static Transform _loadingPanel;
        private static TMP_InputField _errorText;

        private Transform _activeForm;
        private Transform _loginForm;
        private Transform _mainPanel;
        private Transform _registerForm;

        private AuthenticationState _state;

        private void Awake()
        {
            _mainPanel = transform.Find("mainPanel");
            _registerForm = transform.Find("register");
            _loginForm = transform.Find("login");

            _loadingPanel = transform.Find("loadingPanel");
            _errorText = transform.Find("errorText").GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            _registerForm.gameObject.SetActive(false);
            _loginForm.gameObject.SetActive(false);

            _loadingPanel.gameObject.SetActive(false);
            _errorText.gameObject.SetActive(false);
        }

        public void SetAuthenticationTo(int formState)
        {
            _state = (AuthenticationState)formState;

            switch (_state)
            {
                case AuthenticationState.Default:
                    _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text = "";
                    _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text = "";
                    if (_activeForm == _registerForm)
                    {
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text = "";
                        _activeForm.Find("rePasswordField").GetComponent<TMP_InputField>().text = "";
                    }

                    _activeForm.gameObject.SetActive(false);
                    _mainPanel.gameObject.SetActive(true);
                    break;
                case AuthenticationState.Register:
                    _mainPanel.gameObject.SetActive(false);
                    _registerForm.gameObject.SetActive(true);

                    _activeForm = _registerForm;
                    break;
                case AuthenticationState.Login:
                    _mainPanel.gameObject.SetActive(false);
                    _loginForm.gameObject.SetActive(true);

                    _activeForm = _loginForm;
                    break;
            }
        }

        public void Submit()
        {
            _loadingPanel.gameObject.SetActive(true);
            if (_activeForm == _registerForm)
            {
                if (!_activeForm.Find("passwordField").GetComponent<TMP_InputField>().text
                        .Equals(_activeForm.Find("rePasswordField").GetComponent<TMP_InputField>().text)) 
                {
                    ShowLoginError("Password must correspond to the password verification.");
                } 
                else 
                {
                    AuthenticationManager.Instance.Submit(
                        _state,
                        _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text,
                        _activeForm.Find("emailField").GetComponent<TMP_InputField>().text
                    );
                }
            }
            else
            {
                AuthenticationManager.Instance.Submit(
                    _state,
                    _activeForm.Find("usernameField").GetComponent<TMP_InputField>().text,
                    _activeForm.Find("passwordField").GetComponent<TMP_InputField>().text
                );
            }
        }

        private static void ShowLoginError(string message)
        {
            _loadingPanel.gameObject.SetActive(false);
            _errorText.gameObject.SetActive(true);
            _errorText.text = message;
        }

        public void VerifyInputs()
        {
            //Todo
        }
    }
}