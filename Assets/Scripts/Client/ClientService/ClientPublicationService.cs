﻿using System.Collections;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientPublicationService : ClientServiceBase
    {
        [Header("Services")]
        [SerializeField]
        private ClientUserService userService;
        [SerializeField]
        private ClientCommentService commentService;
        [Header("Publications Settings")]
        [SerializeField]
        private new Camera camera;
        [SerializeField]
        private RectTransform publicationsParent;
        [SerializeField]
        private PublicationDisplay publicationPrefab;
        [SerializeField, Min(0.1f)]
        private float refreshUpdateDelay = 3f;

        private float lastRefreshTime;
        private bool initilized;

        private PublicationDisplayBuilder publicationDisplayBuilder;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            publicationDisplayBuilder = new PublicationDisplayBuilder(connection, publicationsParent, commentService, publicationPrefab);
            publicationDisplayBuilder.Load();
            initilized = true;
        }

        private void Update()
        {
            if (initilized
                && lastRefreshTime + refreshUpdateDelay < Time.timeSinceLevelLoad)
            {
                Vector3[] corners = new Vector3[4];
                publicationsParent.GetWorldCorners(corners);
                bool updateFromTop = camera.WorldToViewportPoint(corners[1]).y < 0.95f;
                bool updateFromBottom = camera.WorldToViewportPoint(corners[0]).y > -0.5f;

                if (updateFromTop || updateFromBottom)
                {
                    lastRefreshTime = Time.timeSinceLevelLoad;
                    publicationDisplayBuilder.Load(updateFromTop ? GetDataMode.After : GetDataMode.Before);
                }
            }
        }

        public void CreatePublication()
        {
            NativeGallery.GetImageFromGallery(Callback, "Load publication image");

            void Callback(string photoPath)
            {
                Texture2D textureFrom = NativeGallery.LoadImageAtPath(photoPath, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                connection.PublicationService.PostPublication(userService.CurrentUser.Id, textureFrom.EncodeToJPG());
            }
        }
    }
}