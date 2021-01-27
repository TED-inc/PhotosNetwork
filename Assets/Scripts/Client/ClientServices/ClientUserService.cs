using System;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientUserService : ClientServiceBase, IClientUserService
    {
        public override ClientServiceType Type => ClientServiceType.User;
        public User CurrentUser { get; private set; }
        public event Notify OnUserChanged;

        private new ClientUserServiceSerialization serviceSerialization;


        public void LogOut()
        {
            CurrentUser = null;
            serviceSerialization.loginAndRegisterPage.SetActive(true);
            PlayerPrefs.SetInt(nameof(CurrentUser), -1);
            OnUserChanged?.Invoke();
        }

        public void LogIn() =>
            RequestUser(connection.UserService.LogIn, "User not exist");

        public void Register() =>
            RequestUser(connection.UserService.Register, "User already exist");

        private void RequestUser(Action<string, UserCallback> request, string onErrorMessage)
        {
            serviceSerialization.errorLabel.gameObject.SetActive(false);
            serviceSerialization.connectionOverlay.SetActive(true);
            request.Invoke(serviceSerialization.usernameInputField.text, Callback);

            void Callback(User user, Result result)
            {
                if (result == Result.Complete)
                {
                    CurrentUser = user;
                    PlayerPrefs.SetInt(nameof(CurrentUser), user.Id);
                    serviceSerialization.loginAndRegisterPage.SetActive(false);
                }
                else
                {
                    serviceSerialization.errorLabel.gameObject.SetActive(true);
                    serviceSerialization.errorLabel.text = onErrorMessage;
                }

                serviceSerialization.connectionOverlay.SetActive(false);
            }
            OnUserChanged?.Invoke();
        }

        public ClientUserService(IServerConnection connection, ClientUserServiceSerialization serviceSerialization) : base(connection, serviceSerialization)
        {
            this.serviceSerialization = serviceSerialization;

            int loginedId = PlayerPrefs.GetInt(nameof(CurrentUser), -1);
            if (loginedId != -1)
            {
                serviceSerialization.connectionOverlay.SetActive(true);
                connection.UserService.StartLoggedIn(loginedId, Callback);
            }
            else
                serviceSerialization.loginAndRegisterPage.SetActive(true);



            void Callback(User user, Result result)
            {
                if (result == Result.Complete)
                    CurrentUser = user;
                else
                    serviceSerialization.loginAndRegisterPage.SetActive(true);

                serviceSerialization.connectionOverlay.SetActive(false);
                OnUserChanged?.Invoke();
            }
        }
    }
}