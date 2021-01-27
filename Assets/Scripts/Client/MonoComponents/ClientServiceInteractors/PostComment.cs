using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class PostComment : MonoBehaviour
    {
        [SerializeField]
        private ClientRunner clientRunner;

        public void Post() =>
            clientRunner.GetService<IClientCommentService>().PostComment();

    }
}