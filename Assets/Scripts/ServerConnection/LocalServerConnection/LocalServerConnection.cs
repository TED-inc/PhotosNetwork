using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalServerConnection : IServerConnection
    {
        public IUserService UserService { get; private set; }
        public IPublicationService PublicationService { get; private set; }
        public ICommentService CommentService { get; private set; }

        private SQLiteConnection connection;
        private const string DatabaseName = "local.db";

        public LocalServerConnection()
        {
            connection = CreateConnection();

            UserService = new LocalUserService(connection);
            PublicationService = new LocalPublicationService(connection);
            CommentService = new LocalCommentService(connection);



            SQLiteConnection CreateConnection()
            {
                string localDbPath = Application.streamingAssetsPath + "/" + DatabaseName;
                if (!Application.isEditor)
                {
                    string persistantDbPath = Application.persistentDataPath + "/" + DatabaseName;

                    if (!File.Exists(persistantDbPath))
                    {
                        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer)
                        {
                            UnityWebRequest loadDb = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + DatabaseName);
                            loadDb.SendWebRequest();
                            while (!loadDb.isDone) { }
                            File.WriteAllBytes(persistantDbPath, loadDb.downloadHandler.data);
                        }
                        else
                        File.Copy(Application.streamingAssetsPath + "/" + DatabaseName, persistantDbPath);
                    }

                    localDbPath = persistantDbPath;
                }

                return new SQLiteConnection(localDbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            }
        }
    }
}