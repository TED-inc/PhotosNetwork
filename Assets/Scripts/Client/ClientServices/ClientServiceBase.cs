namespace TEDinc.PhotosNetwork
{
    public delegate void Notify();

    public abstract class ClientServiceBase : IClientServiceBase
    {
        protected IServerConnection connection;

        public event SetActivePage onPageActiveStateChange;

        public void ShowPage() =>
            onPageActiveStateChange?.Invoke(true);

        public void HidePage() =>
            onPageActiveStateChange?.Invoke(false); 

        public ClientServiceBase(IServerConnection connection) =>
            this.connection = connection;
    }
}