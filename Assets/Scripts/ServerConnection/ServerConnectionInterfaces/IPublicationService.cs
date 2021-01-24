namespace TEDinc.PhotosNetwork
{
    public interface IPublicationService
    {
        void GetPublications(int count, GetPublicationsCallback callback);
        void GetPublications(int fromPublicationId, int count, GetDataMode mode, GetPublicationsCallback callback);
        void PostPublication(int userId, byte[] photoData, ResultCallback callback = null);
        void GetPhoto(int photoId, GetPhotoCallback callback);
    }
}