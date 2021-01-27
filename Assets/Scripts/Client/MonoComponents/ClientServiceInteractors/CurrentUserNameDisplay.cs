using System.Collections;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class CurrentUserNameDisplay : MonoBehaviour
    {
        [SerializeField]
        private ClientRunner clientRunner;
        [SerializeField]
        private TMP_Text username;

        private IClientUserService clientUserService;

        private IEnumerator Start()
        {
            while (!clientRunner.Initilized)
                yield return new WaitForEndOfFrame();

            clientUserService = clientRunner.GetService<IClientUserService>(); 
            clientUserService.OnUserChanged += RefreshName;
            RefreshName();
        }

        private void OnDestroy() =>
            clientUserService.OnUserChanged -= RefreshName;

        private void RefreshName() =>
            username.text = clientUserService.CurrentUser?.Username;
    }
}