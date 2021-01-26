namespace TEDinc.PhotosNetwork
{
    public interface IClientPublicationService : IClientServiceBase
    {
        void CreatePublication();
        void Load(GetDataMode mode);
    }
}