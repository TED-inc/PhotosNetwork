using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public sealed class ClientRunner : MonoBehaviour
    {
        public bool Initilized { get; private set; }

        [SerializeField]
        private ClientUserServiceSerialization userServiceSerialization;
        [SerializeField]
        private ClientCommentServiceSerialization commentServiceSerialization;
        [SerializeField]
        private ClientPublicationServiceSerialization publicationServiceSerialization;
        private IServerConnection serverConnection;

        private IClientUserService userService;
        private IClientCommentService commentService;
        private IClientPublicationService publicationService;
        private Dictionary<ClientServiceType, IClientServiceBase> services = 
            new Dictionary<ClientServiceType, IClientServiceBase>(Enum.GetValues(typeof(ClientServiceType)).Length);



        private void Start()
        {
            serverConnection = new LocalServerConnection();
            serverConnection.OnServerConnectStateChange += OnSetup;
            StartCoroutine(serverConnection.Setup());
        }

        private void OnSetup(ServerConnectionState state)
        {
            if (state != ServerConnectionState.Connected)
                return;

            serverConnection.OnServerConnectStateChange -= OnSetup;

            userService = new ClientUserService(serverConnection, userServiceSerialization);
            commentService = new ClientCommentService(serverConnection, userService, commentServiceSerialization);
            publicationService = new ClientPublicationService(serverConnection, userService, commentService, publicationServiceSerialization);

            services.Add(userService.Type, userService);
            services.Add(commentService.Type, commentService);
            services.Add(publicationService.Type, publicationService);

            Initilized = true;
        }

        public T GetService<T>() where T : IClientServiceBase
        {
            foreach (var service in services.Values)
                if (service is T)
                    return (T)service;

            throw new NotImplementedException();
        }

        public T GetService<T>(ClientServiceType type) where T : IClientServiceBase =>
            (T)services[type];
    }


    public enum ClientServiceType
    {
        User,
        Comment,
        Publication,
    }
}