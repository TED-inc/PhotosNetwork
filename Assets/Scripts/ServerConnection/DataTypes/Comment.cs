using System;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class Comment
    {
        [PrimaryKey, Unique, AutoIncrement]
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int PublicationId { get; private set; }
        public long DataTimeUTC { get; private set; }
        public string Message { get; private set; }

        public Comment(int userId, int publicationId, long dataTimeUTC, string message)
        {
            UserId = userId;
            PublicationId = publicationId;
            DataTimeUTC = dataTimeUTC;
            Message = message;
        }
    }
}