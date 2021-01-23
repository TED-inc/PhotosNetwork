using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class User
    {
        [PrimaryKey, Unique, AutoIncrement]
        public int Id { get; private set; }
        [Unique]
        public string Username { get; private set; }

        public User() { }

        public User(string username)
        {
            Username = username;
        }
    }
}