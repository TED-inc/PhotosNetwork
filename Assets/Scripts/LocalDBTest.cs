using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public class LocalDBTest : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        public SQLiteConnection connection
        {
            get
            {
                if (_connection == null)
                    _connection = LoadDB();
                return _connection;
            }
        }

        private SQLiteConnection _connection;

        private const string DatabaseName = "local.db";

        private SQLiteConnection LoadDB()
        {
#if UNITY_EDITOR
            string dbPath = Application.streamingAssetsPath + "/" + DatabaseName; 
#else
            string persistantDBPath = Application.persistentDataPath + "/" + DatabaseName;

            if (!File.Exists(persistantDBPath))
            {
#if UNITY_ANDROID
                UnityWebRequest loadDb = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + DatabaseName);
                    loadDb.SendWebRequest();
                while (!loadDb.isDone) { }
                File.WriteAllBytes(persistantDBPath, loadDb.downloadHandler.data);
#else
                File.Copy(Application.streamingAssetsPath + "/" + DatabaseName, persistantDBPath);
#endif
            }

            string dbPath = persistantDBPath;
#endif
            return new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }

        public class User
        {
            [PrimaryKey, Unique, AutoIncrement]
            public int Id { get; private set; }
            [Unique]
            public string Username { get; private set; }

            public override string ToString()
            {
                return $"{Id} : {Username}";
            }
        }

        public class Publication
        {
            [PrimaryKey, Unique, AutoIncrement]
            public int Id { get; private set; }
            public int UserId { get; private set; }
            public long DataTimeUTC { get; private set; }
            public string Message { get; private set; }
            public byte[] PhotoData { get; private set; }

            public Publication() { }

            public Publication(int userId, string message, byte[] photoData)
            {
                UserId = userId;
                Message = message;
                PhotoData = photoData;
                DataTimeUTC = DateTime.UtcNow.Ticks;
            }
        }
    }
}