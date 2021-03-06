﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public sealed class PublicationDisplay : MonoBehaviour
    {
        [SerializeField]
        private RawImage image;
        [SerializeField]
        private TMP_Text usernameLabel;

        private int publicationId;
        private OpenComments openComment;

        public void OpenComments() =>
            openComment.Invoke(publicationId);

        public void Setup(string username, int publicationId, OpenComments onComments)
        {
            usernameLabel.text = username;
            this.publicationId = publicationId;
            this.openComment = onComments;
            name = $"Publication[{username},{publicationId}]";
        }

        public void SetTexture(Texture2D texture)
        {
            image.texture = texture;
            name += $" {texture.width}x{texture.height}";
            ResizeTransformToFitTexture();



            void ResizeTransformToFitTexture()
            {
                RectTransform rectTransform = (transform as RectTransform);

                image.rectTransform.sizeDelta = new Vector2(
                    rectTransform.rect.width,
                    (float)texture.height / texture.width * rectTransform.rect.width);

                rectTransform.sizeDelta += Vector2.up * image.rectTransform.sizeDelta.y;
            }
        }
    }
}