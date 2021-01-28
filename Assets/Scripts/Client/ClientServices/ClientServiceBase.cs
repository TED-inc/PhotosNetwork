using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public abstract class ClientServiceBase : IClientServiceBase
    {
        protected IServerConnection connection;
        protected ClientServiceSerializationBase serviceSerialization;

        public abstract ClientServiceType Type { get; }

        public void SetActivePage(bool enabled)
        {
            foreach (GameObject item in serviceSerialization.pageEnableItems)
                item.SetActive(enabled);
        }

        public ClientServiceBase(IServerConnection connection, ClientServiceSerializationBase serviceSerialization)
        {
            this.connection = connection;
            this.serviceSerialization = serviceSerialization;
        }
    }
}