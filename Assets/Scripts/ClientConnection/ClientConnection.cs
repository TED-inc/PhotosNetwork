using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public class ClientConnection : MonoBehaviour
    {
        public IServerConnection serverConnection { get; private set; }

        [SerializeField]
        private GameObject connectionOverlay;

        private IEnumerator Start()
        {
            connectionOverlay.SetActive(true);
            serverConnection = new LocalServerConnection();

            yield return serverConnection.Setup();
            connectionOverlay.SetActive(false);
        }
    }
}