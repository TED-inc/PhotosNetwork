using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientPublicationService : ClientServiceBase, IClientPublicationService
    {
        public override ClientServiceType Type => ClientServiceType.Publication;

        private IClientUserService userService;
        private new ClientPublicationServiceSerialization serviceSerialization;
        private PublicationDisplayBuilder publicationDisplayBuilder;

        private float lastRefreshTime;
        private bool initilized;


        private void Update()
        {
            if (initilized
                && lastRefreshTime + serviceSerialization.refreshUpdateDelay < Time.timeSinceLevelLoad)
            {
                Vector3[] corners = new Vector3[4];
                serviceSerialization.publicationsParent.GetWorldCorners(corners);
                bool updateFromTop = serviceSerialization.camera.WorldToViewportPoint(corners[1]).y < 0.95f;
                bool updateFromBottom = serviceSerialization.camera.WorldToViewportPoint(corners[0]).y > -0.5f;

                if (updateFromTop || updateFromBottom)
                {
                    lastRefreshTime = Time.timeSinceLevelLoad;
                    publicationDisplayBuilder.Load(updateFromTop ? GetDataMode.After : GetDataMode.Before);
                }
            }
        }

        public void Load(GetDataMode mode) =>
            publicationDisplayBuilder.Load(mode);

        public void CreatePublication()
        {
            NativeGallery.GetImageFromGallery(Callback, "Load publication image");

            void Callback(string photoPath)
            {
                Texture2D textureFrom = NativeGallery.LoadImageAtPath(photoPath, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                connection.PublicationService.PostPublication(userService.CurrentUser.Id, textureFrom.EncodeToJPG());
            }
        }

        public ClientPublicationService(IServerConnection connection, IClientUserService userService, IClientCommentService commentService, ClientPublicationServiceSerialization serviceSerialization) : base(connection, serviceSerialization)
        {
            this.userService = userService;
            this.serviceSerialization = serviceSerialization;

            publicationDisplayBuilder = new PublicationDisplayBuilder(connection, serviceSerialization.publicationsParent, commentService, serviceSerialization.publicationPrefab);
            publicationDisplayBuilder.Load();
            initilized = true;
        }
    }
}