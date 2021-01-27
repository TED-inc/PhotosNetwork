using System;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public sealed class ClientPublicationServiceSerialization : ClientServiceSerializationBase
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private RectTransform _publicationsParent;
        [SerializeField]
        private PublicationDisplay _publicationPrefab;
        [SerializeField, Min(0.1f)]
        private float _refreshUpdateDelay = 1f;

        public Camera camera => _camera;
        public RectTransform publicationsParent => _publicationsParent;
        public PublicationDisplay publicationPrefab => _publicationPrefab;
        public float refreshUpdateDelay => _refreshUpdateDelay;
    }
}
