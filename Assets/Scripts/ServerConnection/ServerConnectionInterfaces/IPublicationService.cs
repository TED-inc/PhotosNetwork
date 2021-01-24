namespace TEDinc.PhotosNetwork
{
    public interface IPublicationService
    {
        void GetPublications(int count, GetPublicationsCallback callback);
        void GetPublications(int fromPublicationId, int count, GetDataMode mode, GetPublicationsCallback callback);
        void PostPublication(int userId, string photoPath, ResultCallback callback);
    }
}