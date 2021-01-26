using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalServerConnection : IServerConnection
    {
        public event OnServerConnectStateChange OnServerConnectStateChange;
        public ServerConnectionState CurrentState { get; private set; }
        public IUserService UserService { get; private set; }
        public IPublicationService PublicationService { get; private set; }
        public ICommentService CommentService { get; private set; }

        private SQLiteConnection connection;
        private const string DatabaseName = "local.db";


        public IEnumerator Setup()
        {
            SetState(ServerConnectionState.Pending);

            string localDbPath = Application.streamingAssetsPath + "/" + DatabaseName;
            if (!Application.isEditor)
            {
                string persistantDbPath = Application.persistentDataPath + "/" + DatabaseName;

                if (!File.Exists(persistantDbPath))
                {
                    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        UnityWebRequest loadDb = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + DatabaseName);
                        yield return loadDb.SendWebRequest();
                        File.WriteAllBytes(persistantDbPath, loadDb.downloadHandler.data);
                    }
                    else
                        File.Copy(Application.streamingAssetsPath + "/" + DatabaseName, persistantDbPath);
                }

                localDbPath = persistantDbPath;
            }

            connection = new SQLiteConnection(localDbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);


            UserService = new LocalUserService(connection);
            PublicationService = new LocalPublicationService(connection);
            CommentService = new LocalCommentService(connection);

            SetState(ServerConnectionState.Connected);
        }

        private void SetState(ServerConnectionState state)
        {
            bool invokeStateChange = state != CurrentState;
            CurrentState = state;
            if (invokeStateChange)
                OnServerConnectStateChange?.Invoke(CurrentState);
        }
    }
}