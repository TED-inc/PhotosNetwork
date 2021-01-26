using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientUserService : ClientServiceBase, IClientUserService
    {
        [Header("Login and Register")]
        [SerializeField]
        private GameObject connectionOverlay;
        [SerializeField]
        private GameObject loginAndRegisterPage;
        [SerializeField]
        private TMP_InputField usernameInputField;
        [SerializeField]
        private TMP_Text errorLabel;

        public User CurrentUser { get; private set; }
        public event Notify OnUserChanged;


        public void LogOut()
        {
            CurrentUser = null;
            loginAndRegisterPage.SetActive(true);
            PlayerPrefs.SetInt(nameof(CurrentUser), -1);
            OnUserChanged?.Invoke();
        }

        public void LogIn() =>
            RequestUser(connection.UserService.LogIn, "User not exist");

        public void Register() =>
            RequestUser(connection.UserService.Register, "User already exist");

        private void RequestUser(Action<string, UserCallback> request, string onErrorMessage)
        {
            errorLabel.gameObject.SetActive(false);
            connectionOverlay.SetActive(true);
            request.Invoke(usernameInputField.text, Callback);

            void Callback(User user, Result result)
            {
                if (result == Result.Complete)
                {
                    CurrentUser = user;
                    PlayerPrefs.SetInt(nameof(CurrentUser), user.Id);
                    loginAndRegisterPage.SetActive(false);
                }
                else
                {
                    errorLabel.gameObject.SetActive(true);
                    errorLabel.text = onErrorMessage;
                }

                connectionOverlay.SetActive(false);
            }
            OnUserChanged?.Invoke();
        }

        public ClientUserService(IServerConnection connection) : base(connection)
        {
            int loginedId = PlayerPrefs.GetInt(nameof(CurrentUser), -1);
            if (loginedId != -1)
            {
                connectionOverlay.SetActive(true);
                connection.UserService.StartLoggedIn(loginedId, Callback);
            }
            else
                loginAndRegisterPage.SetActive(true);



            void Callback(User user, Result result)
            {
                if (result == Result.Complete)
                    CurrentUser = user;
                else
                    loginAndRegisterPage.SetActive(true);

                connectionOverlay.SetActive(false);
                OnUserChanged?.Invoke();
            }
        }
    }
}