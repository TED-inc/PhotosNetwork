namespace TEDinc.PhotosNetwork
{
    public delegate (Publication[] user, Result result) GetPublicationsCallback();
    public interface IPublicationService
    {
        void Setup(IUserService userService);

        void GetPublications(int fromPublicationId, int count, GetDataMode mode, GetPublicationsCallback callback);
    }
}