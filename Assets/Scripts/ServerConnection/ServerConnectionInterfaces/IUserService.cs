namespace TEDinc.PhotosNetwork
{
    public delegate (User user, Result result) LogInCallback();
    public interface IUserService
    {
        User CurrentUser { get; }

        void Setup();

        bool IsUserLoggedIn();
        void LogOut();
        void LogIn(int userId, LogInCallback callback);
    }
}