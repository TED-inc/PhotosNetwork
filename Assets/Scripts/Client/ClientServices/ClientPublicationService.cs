using System.IO;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientPublicationService : ClientServiceBase, IClientPublicationService
    {
        public override ClientServiceType Type => ClientServiceType.Publication;

        private IClientUserService userService;
        private PublicationDisplayBuilder publicationDisplayBuilder;


        public void Load(GetDataMode mode) =>
            publicationDisplayBuilder.Load(mode);

        public void CreatePublication()
        {
            NativeGallery.GetImageFromGallery(Callback, "Load publication image");

            void Callback(string photoPath)
            {
                if (string.IsNullOrEmpty(photoPath) || !File.Exists(photoPath))
                    return;

                Texture2D textureFrom = NativeGallery.LoadImageAtPath(photoPath, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                connection.PublicationService.PostPublication(userService.CurrentUser.Id, textureFrom.EncodeToJPG());
            }
        }

        public ClientPublicationService(IServerConnection connection, IClientUserService userService, IClientCommentService commentService, ClientPublicationServiceSerialization serviceSerialization) : base(connection, serviceSerialization)
        {
            this.userService = userService;

            publicationDisplayBuilder = new PublicationDisplayBuilder(connection, commentService, serviceSerialization);
            publicationDisplayBuilder.Load();
        }
    }
}