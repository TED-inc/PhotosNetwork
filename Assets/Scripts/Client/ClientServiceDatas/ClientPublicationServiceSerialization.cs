using System;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public sealed class ClientPublicationServiceSerialization : ClientServiceSerializationBase
    {
        [SerializeField]
        private RectTransform _publicationsParent;
        [SerializeField]
        private PublicationDisplay _publicationPrefab;
        

        public RectTransform publicationsParent => _publicationsParent;
        public PublicationDisplay publicationPrefab => _publicationPrefab;
    }
}
