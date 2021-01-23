﻿using System.IO;
using UnityEngine;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class PhotoData
    {
        [PrimaryKey, Unique, AutoIncrement]
        public int Id { get; private set; }
        public byte[] Data { get; private set; }

        private string DirectoryCashePath => $"{Application.temporaryCachePath}/Images";
        private string FilePathInCahse => $"{DirectoryCashePath}/{Id}.jpg";

        public void SaveToCahse()
        {
            if (!Directory.Exists(DirectoryCashePath))
                Directory.CreateDirectory(DirectoryCashePath);
        }

        public void ConvertTextureToData(Texture2D texture)
        {

        }
    }
}