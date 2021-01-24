using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalPublicationService : LocalServiceBase, IPublicationService
    {
        public void GetPublications(int count, GetPublicationsCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void GetPublications(int fromPublicationId, int count, GetDataMode mode, GetPublicationsCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void PostPublication(int userId, byte[] photoData, ResultCallback callback = null)
        {
            PhotoData photo = new PhotoData(photoData);
            connection.Insert(photo);
            connection.Insert(new Publication(userId, photo.Id));
            callback?.Invoke(Result.Complete);
        }

        public LocalPublicationService(SQLiteConnection connection) : base(connection) { }
    }
}