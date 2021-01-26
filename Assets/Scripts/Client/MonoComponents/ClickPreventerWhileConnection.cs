using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClickPreventerWhileConnection : MonoBehaviour
    {
        [SerializeField]
        private GameObject clickPreventor;
        public static ClickPreventerWhileConnection Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            ServerConnection.connection.OnServerConnectStateChange += (ServerConnectionState state) => 
                ChangePreventorState(state != ServerConnectionState.Connected);
        }

        public void ChangePreventorState(bool enabled) =>
            clickPreventor.SetActive(enabled);
    }
}