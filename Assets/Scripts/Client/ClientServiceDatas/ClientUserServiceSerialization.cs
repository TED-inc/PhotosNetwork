using System;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public sealed class ClientUserServiceSerialization : ClientServiceSerializationBase
    {
        [SerializeField]
        private GameObject _connectionOverlay;
        [SerializeField]
        private GameObject _loginAndRegisterPage;
        [SerializeField]
        private TMP_InputField _usernameInputField;
        [SerializeField]
        private TMP_Text _errorLabel;

        public GameObject connectionOverlay => _connectionOverlay;
        public GameObject loginAndRegisterPage => _loginAndRegisterPage;
        public TMP_InputField usernameInputField => _usernameInputField;
        public TMP_Text errorLabel => _errorLabel;
    }
}