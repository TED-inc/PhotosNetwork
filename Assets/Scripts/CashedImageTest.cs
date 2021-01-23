using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public class CashedImageTest : MonoBehaviour
    {
        [SerializeField]
        private LocalDBTest localDB;
        [SerializeField]
        private RectTransform imagesParent;
        [SerializeField]
        private TMP_Text label;

        private Dictionary<int, RawImage> images = new Dictionary<int, RawImage>();

        private string CashePath => $"{Application.temporaryCachePath}/Images/";

        private string FileInCashePath(string fileName) => 
            $"{CashePath}{fileName}.jpg";

        public void SaveToCasheFromLocalDB()
        {
            if (!Directory.Exists(CashePath))
                Directory.CreateDirectory(CashePath);
            foreach (LocalDBTest.Publication publication in localDB.connection.Table<LocalDBTest.Publication>())
            {
                label.text += $"publication.Id {publication.Id}\n";
                File.WriteAllBytes(FileInCashePath(publication.Id.ToString()), publication.PhotoData);
            }
            label.text += "!publ!\n";
        }

        public void LoadFromCasheAndDisplay()
        {
            if (!Directory.Exists(CashePath))
                Directory.CreateDirectory(CashePath);
            foreach (string cashedImagePath in Directory.GetFiles(CashePath))
            {
                int id = Convert.ToInt32(Path.GetFileNameWithoutExtension(cashedImagePath));
                label.text += $"cashed.Id {id}\n";
                RawImage rawImage;
                Texture2D texture = new Texture2D(0, 0);

                if (images.ContainsKey(id))
                    rawImage = images[id];
                else
                {
                    GameObject imageParent = new GameObject($"img{id}");
                    imageParent.transform.SetParent(imagesParent);
                    imageParent.transform.localScale = Vector3.one;
                    rawImage = imageParent.AddComponent<RawImage>();
                    images.Add(id, rawImage);
                }
                
                texture.LoadImage(File.ReadAllBytes(cashedImagePath));
                rawImage.texture = texture;
                rawImage.rectTransform.sizeDelta = new Vector2(
                    imagesParent.rect.width, 
                    (float)texture.height / texture.width * imagesParent.rect.width);
            }
            label.text += "!cash!\n";
        }
    }
}