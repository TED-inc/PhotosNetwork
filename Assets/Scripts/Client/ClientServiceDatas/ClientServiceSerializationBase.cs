using System;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public abstract class ClientServiceSerializationBase
    {
        [SerializeField]
        private GameObject[] _pageEnableItems;

        public GameObject[] pageEnableItems => _pageEnableItems;
    }
}