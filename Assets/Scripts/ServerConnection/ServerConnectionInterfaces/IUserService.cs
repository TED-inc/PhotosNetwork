namespace TEDinc.PhotosNetwork
{
    public interface IUserService
    {
        void LogIn(string username, UserCallback callback);
        void Register(string username, UserCallback callback);
    }
}