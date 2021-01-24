using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class LocalUserService : LocalServiceBase, IUserService
    {
        public void LogIn(string username, UserCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void Register(string username, UserCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public LocalUserService(SQLiteConnection connection) : base(connection) { }
    }
}