using System.Collections;
using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public abstract class ClientServiceInteractorBase : MonoBehaviour
    {
        [SerializeField]
        private ClientRunner _clientRunner;

        protected ClientRunner clientRunner => _clientRunner;

        private IEnumerator Start()
        {
            while (!clientRunner.Initilized)
                yield return new WaitForSeconds(0.1f);

            OnInit();
        }

        protected virtual void OnInit() { }
    }
}