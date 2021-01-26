using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientRunner : MonoBehaviour
    {
        [SerializeField]
        private ClientUserServiceSerializable userServiceSerializable;

        private IServerConnection serverConnection;

        private IClientUserService userService;
        private IClientCommentService commentService;
        private IClientPublicationService publicationService;



        private void Start()
        {
            serverConnection = new LocalServerConnection();
            serverConnection.OnServerConnectStateChange += OnSetup;
            StartCoroutine(ServerConnection.connection.Setup());



            void OnSetup(ServerConnectionState state)
            {
                if (state != ServerConnectionState.Connected)
                    return;

                serverConnection.OnServerConnectStateChange -= OnSetup;

                userService = new ClientUserService(serverConnection);
                commentService = new ClientCommentService(serverConnection, userService);
                publicationService = new ClientPublicationService(serverConnection, userService, commentService);
            }
        }
    }
}