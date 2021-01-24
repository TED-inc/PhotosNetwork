using System.IO;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientPublicationService : ClientServiceBase
    {
        [SerializeField]
        private ClientUserService userService;


        //private string DirectoryCashePath => $"{Application.temporaryCachePath}/Images";
        //private string FilePathInCahse => $"{DirectoryCashePath}/{Id}.jpg";

        //public void SaveToCahse()
        //{
        //    if (!Directory.Exists(DirectoryCashePath))
        //        Directory.CreateDirectory(DirectoryCashePath);
        //    File.WriteAllBytes(FilePathInCahse, Data);
        //}

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