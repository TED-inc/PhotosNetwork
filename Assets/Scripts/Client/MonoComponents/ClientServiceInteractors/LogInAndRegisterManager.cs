using System.Collections;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class LogInAndRegisterManager : ClientServiceInteractorBase
    {
        [SerializeField]
        private TMP_InputField usernameInputField;
        [SerializeField]
        private TMP_Text errorLabel;

        private IClientUserService clientUserService;

        protected override void OnInit() =>
            clientUserService = clientRunner.GetService<IClientUserService>();

        public void LogIn() =>
            clientUserService.LogIn(usernameInputField.text, UserRequestCallback);

        public void Register() =>
            clientUserService.Register(usernameInputField.text, UserRequestCallback);

        private void UserRequestCallback(Result result, string errorMessage)
        {
            if (result == Result.Complete)
                usernameInputField.text = "";
            errorLabel.gameObject.SetActive(result != Result.Complete);
            errorLabel.text = errorMessage;
        }
    }
}