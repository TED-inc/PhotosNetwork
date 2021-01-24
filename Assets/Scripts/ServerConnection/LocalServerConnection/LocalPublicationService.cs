using System.Linq;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalPublicationService : LocalServiceBase, IPublicationService
    {
        public void GetPublications(int count, GetPublicationsCallback callback)
        {
            int i = 0;
            List<(Publication, User)> publications = new List<(Publication, User)>(count);
            foreach (Publication pubication in connection.Table<Publication>().OrderBy(pubication => pubication.DataTimeUTC).Reverse())
            {
                if (i++ < count)
                {
                    User user = connection.Table<User>().Where(u => u.Id == pubication.UserId).FirstOrDefault();
                    publications.Add((pubication, user));
                }
                else
                    break;
            }
            callback.Invoke(publications.ToArray(), publications.Count == count ? Result.Complete : Result.ParticularlyComplete);
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

        public void GetPhoto(int photoId, GetPhotoCallback callback)
        {
            PhotoData data = connection.Table<PhotoData>().Where((photoData) => photoData.Id == photoId).FirstOrDefault();
            callback.Invoke(data, data == null ? Result.Failed : Result.Complete);
        }

        public LocalPublicationService(SQLiteConnection connection) : base(connection) { }
    }
}