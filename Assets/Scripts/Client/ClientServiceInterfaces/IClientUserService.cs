namespace TEDinc.PhotosNetwork
{
    public interface IClientUserService : IClientServiceBase
    {
        User CurrentUser { get; }
        event Notify OnUserChanged;

        void LogOut();
        void LogIn(string username, UserRequestCallback userRequestCallback);
        void Register(string username, UserRequestCallback userRequestCallback);
    }
}