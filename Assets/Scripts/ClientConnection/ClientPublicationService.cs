using System.Collections;
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
        private PublicationInstance publicationPrefab;
        [SerializeField, Min(0.1f)]
        private float refreshUpdateDelay = 3f;

        private float lastRefreshTime;
        private bool initilized;

        private PublicationInstanceFactory publicationFactory;

        private IEnumerator Start()
        {
            while(client.serverConnection == null)
                yield return new WaitForSecondsRealtime(0.1f);

            publicationFactory = new PublicationInstanceFactory(client.serverConnection, publicationsParent, commentService, publicationPrefab);
            publicationFactory.Load();
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
                    publicationFactory.Load(updateFromTop ? GetDataMode.After : GetDataMode.Before);
                }
            }
        }

        public void CreatePublication()
        {
            NativeGallery.GetImageFromGallery(Callback, "Load publication image");

            void Callback(string photoPath)
            {
                Texture2D textureFrom = NativeGallery.LoadImageAtPath(photoPath, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                client.serverConnection.PublicationService.PostPublication(userService.CurrentUser.Id, textureFrom.EncodeToJPG());
            }
        }
    }
}