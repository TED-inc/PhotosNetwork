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
            bool initilized = false;

            Task.Run(() => {
                serverConnection = new LocalServerConnection();
                initilized = true;
            });

            while (!initilized)
                yield return new WaitForSeconds(0.1f);
            connectionOverlay.SetActive(false);
        }
    }
}