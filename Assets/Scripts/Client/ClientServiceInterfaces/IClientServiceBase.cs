namespace TEDinc.PhotosNetwork
{
    public delegate void SetActivePage(bool enabled);
    public interface IClientServiceBase
    {
        ClientServiceType Type { get; }
        void SetActivePage(bool enabled);
    }
}
