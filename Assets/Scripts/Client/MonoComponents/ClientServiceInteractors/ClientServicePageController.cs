using System;
using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class ClientServicePageController : ClientServiceInteractorBase
    {
        [SerializeField]
        private ClientServiceType serviceType;

        public void SetActivePage(bool enabled) =>
            clientRunner.GetService(serviceType).SetActivePage(enabled);

        public void ShowOnlyOnePage()
        {
            foreach (ClientServiceType type in Enum.GetValues(typeof(ClientServiceType)))
                clientRunner.GetService<IClientServiceBase>(type).SetActivePage(serviceType == type);
        }
    }
}