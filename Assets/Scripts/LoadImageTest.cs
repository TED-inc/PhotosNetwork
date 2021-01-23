using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public class LoadImageTest : MonoBehaviour
    {
        [SerializeField]
        private LocalDBTest localDBTest;
        [SerializeField]
        private RawImage image;
        [SerializeField]
        private TMP_Text label;

        private string CashePath => Application.temporaryCachePath + "/f.jpg";

        private void Start()
        {
            //LoadImageFromCashe();
        }

        private void LoadImageFromCashe()
        {
            //if (!File.Exists(CashePath))
            //    return;
            LocalDBTest.Publication publication = localDBTest.connection.Table<LocalDBTest.Publication>().FirstOrDefault();
            if (publication == null || publication.PhotoData == null)
            {
                Debug.LogError("! p.null:" + (publication == null));
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
            Debug.Log("0" + NativeGallery.IsMediaPickerBusy());
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(Callback, "Test loading");
            Debug.Log("1" + NativeGallery.IsMediaPickerBusy() + permission);

            void Callback(string path)
            {
                Debug.Log("2");
                Texture2D textureFrom = NativeGallery.LoadImageAtPath(path, maxSize: 2048, generateMipmaps: false, markTextureNonReadable: false);
                byte[] data = textureFrom.EncodeToJPG();
                label.text += $"path {path}\n";
                localDBTest.connection.Insert(new LocalDBTest.Publication(1, "TestMessage", data));
                label.text += $"count {localDBTest.connection.Table<LocalDBTest.Publication>().Count()}\n";
                LoadImageFromCashe();
                Debug.Log("3");
            }
        }
    }
}