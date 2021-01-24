namespace TEDinc.PhotosNetwork
{
    public interface IUserService
    {
        void StartLoggedIn(int id, UserCallback callback);
        void LogIn(string username, UserCallback callback);
        void Register(string username, UserCallback callback);
    }
}