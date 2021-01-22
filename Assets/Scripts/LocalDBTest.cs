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

        private const string DatabaseName = "local.db";

        public void TestDB()
        {
            label.text = "\n::0::\n";
            try
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
                SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                label.text += "\n::1::\n";
                foreach (var item in connection.Table<User>())
                    label.text += $"{item}\n";
            }
            catch (System.Exception e)
            {
                label.text += e;
            }
            label.text += "\n::2::\n";
        }

        public class User
        {
            public long Id { get; private set; }
            public string Username { get; private set; }

            public override string ToString()
            {
                return $"{Id} : {Username}";
            }
        }
    }
}