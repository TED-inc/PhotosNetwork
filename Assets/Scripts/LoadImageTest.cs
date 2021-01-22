using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TEDinc.PhotosNetwork
{
    public class LoadImageTest : MonoBehaviour
    {
        [SerializeField]
        private LocalDBTest localDBTest;
        [SerializeField]
        private RawImage image;

        private string CashePath => Application.temporaryCachePath + "/f.jpg";

        private void Start()
        {
            LoadImageFromCashe();
        }

        private void LoadImageFromCashe()
        {
            //if (!File.Exists(CashePath))
            //    return;
            LocalDBTest.Publication publication = localDBTest.connection.Table<LocalDBTest.Publication>().FirstOrDefault();
            if (publication == null || publication.PhotoData == null)
            {
                Debug.LogError("!");
                return;
            }
            byte[] data = publication.PhotoData;
            Texture2D textureTo = new Texture2D(0, 0);
            textureTo.LoadImage(File.ReadAllBytes(CashePath));
            image.texture = textureTo;
            image.SetNativeSize();
        }

        public void LoadImageFromGalery()
        {
            NativeGallery.GetImageFromGallery(Callback, "Test loading");

            void Callback(string path)
            {
                Texture2D textureFrom = NativeGallery.LoadImageAtPath(path, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                byte[] data = textureFrom.EncodeToJPG();
                localDBTest.connection.Insert(new LocalDBTest.Publication(1, "TestMessage", data));
                LoadImageFromCashe();
            }
        }
    }
}