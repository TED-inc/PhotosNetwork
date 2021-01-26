using System;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public sealed class ClientUserServiceSerializable
    {
        public GameObject connectionOverlay;
        public GameObject loginAndRegisterPage;
        public TMP_InputField usernameInputField;
        public TMP_Text errorLabel;
    }
}