using System.Collections;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public abstract class ClientServiceBase : MonoBehaviour
    {
        protected IServerConnection connection;
        [SerializeField]
        private GameObject[] items;

        protected virtual IEnumerator Start()
        {
            connection = ServerConnection.connection;

            while (connection.CurrentState != ServerConnectionState.Connected)
                yield return new WaitForSeconds(0.1f);
        }

        public void ShowPage()
        {
            foreach (GameObject item in items)
                item.SetActive(true);
        }

        public void HidePage()
        {
            foreach (GameObject item in items)
                item.SetActive(false);
        }
    }
}