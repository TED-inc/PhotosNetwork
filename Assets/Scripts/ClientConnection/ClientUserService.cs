using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public delegate void Notify();

    public sealed class ClientUserService : ClientServiceBase
    {
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

        private IEnumerator Start()
        {
            Debug.Log("S1");
            while (client.serverConnection == null)
                yield return new WaitForSecondsRealtime(0.1f);
            Debug.Log("S2");

            int loginedId = PlayerPrefs.GetInt(nameof(CurrentUser), -1);
            if (loginedId != -1)
            {
                connectionOverlay.SetActive(true);
                client.serverConnection.UserService.StartLoggedIn(loginedId, Callback);
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
            }
            OnUserChanged?.Invoke();
        }

        public void LogOut()
        {
            CurrentUser = null;
            loginAndRegisterPage.SetActive(true);
            PlayerPrefs.SetInt(nameof(CurrentUser), -1);
            OnUserChanged?.Invoke();
        }

        public void LogIn() =>
            RequestUser(client.serverConnection.UserService.LogIn, "User not exist");

        public void Register() =>
            RequestUser(client.serverConnection.UserService.Register, "User already exist");

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
    }
}