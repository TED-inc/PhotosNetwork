using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalUserService : LocalServiceBase, IUserService
    {
        public void StartLoggedIn(int id, UserCallback callback)
        {
            User user = connection.Table<User>().Where((u) => u.Id == id).FirstOrDefault();
            callback?.Invoke(user, user == null ? Result.Failed : Result.Complete);
        }

        public void LogIn(string username, UserCallback callback)
        {
            User user = connection.Table<User>().Where((u) => u.Username == username).FirstOrDefault();
            callback?.Invoke(user, user == null ? Result.Failed : Result.Complete);
        }

        public void Register(string username, UserCallback callback)
        {
            if (connection.Table<User>().Where((u) => u.Username == username).Count() > 0)
                callback?.Invoke(null, Result.Failed);
            else
            {
                User user = new User(username);
                connection.Insert(user);
                callback?.Invoke(user, Result.Complete);
            }
        }


        public LocalUserService(SQLiteConnection connection) : base(connection) { }
    }
}