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

        public GameObject connectionOverlay => _connectionOverlay;
        public GameObject loginAndRegisterPage => _loginAndRegisterPage;
    }
}