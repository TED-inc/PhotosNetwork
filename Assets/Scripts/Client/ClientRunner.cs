using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientRunner : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ServerConnection.connection.Setup());
        }
    }
}