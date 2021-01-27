using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    [Serializable]
    public sealed class ClientCommentServiceSerialization : ClientServiceSerializationBase
    {
        [SerializeField]
        private RectTransform _commentsParent;
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private CommentDisplay _commentPrefab;
        [SerializeField]
        private TMP_InputField _commentInput;

        public RectTransform commentsParent => _commentsParent;
        public ScrollRect scrollRect => _scrollRect;
        public CommentDisplay commentPrefab => _commentPrefab;
        public TMP_InputField commentInput => _commentInput;
    }
}
