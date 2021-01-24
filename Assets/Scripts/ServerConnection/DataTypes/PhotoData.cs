using System.IO;
using UnityEngine;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class PhotoData
    {
        [PrimaryKey, Unique, AutoIncrement]
        public int Id { get; private set; }
        public byte[] Data { get; private set; }

        public PhotoData() { } // required for loading from SQL

        public PhotoData(byte[] data) =>
            Data = data;
    }
}