using System;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class Publication
    {
        [PrimaryKey, Unique, AutoIncrement]
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public long DataTimeUTC { get; private set; }
        public byte[] PhotoData { get; private set; }

        public Publication() { } // required for loading from SQL

        public Publication(int userId, byte[] photoData)
        {
            UserId = userId;
            PhotoData = photoData;
            DataTimeUTC = DateTime.UtcNow.Ticks;
        }
    }
}