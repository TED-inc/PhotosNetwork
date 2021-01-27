using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class CreatePublication : MonoBehaviour
    {
        [SerializeField]
        private ClientRunner clientRunner;

        public void Create() =>
            clientRunner.GetService<IClientPublicationService>().CreatePublication();
        
    }
}
