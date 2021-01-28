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

        public void LogIn(string username, UserRequestCallback userRequestCallback) =>
            RequestUser(connection.UserService.LogIn, username, userRequestCallback, "User not exist");

        public void Register(string username, UserRequestCallback userRequestCallback) =>
            RequestUser(connection.UserService.Register, username, userRequestCallback, "User already exist");

        private void RequestUser(Action<string, UserCallback> request, string username, UserRequestCallback userRequestCallback, string onErrorMessage)
        {
            serviceSerialization.connectionOverlay.SetActive(true);
            request.Invoke(username, Callback);

            void Callback(User user, Result result)
            {
                if (result == Result.Complete)
                {
                    CurrentUser = user;
                    PlayerPrefs.SetInt(nameof(CurrentUser), user.Id);
                    serviceSerialization.loginAndRegisterPage.SetActive(false);
                    userRequestCallback?.Invoke(result);
                }
                else
                    userRequestCallback?.Invoke(result, onErrorMessage);

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