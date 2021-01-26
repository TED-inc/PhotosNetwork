using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class CurrentUserNameDisplay : MonoBehaviour
    {
        [SerializeField]
        private ClientUserService clientUserService;
        [SerializeField]
        private TMP_Text username;

        private void Awake()
        {
            clientUserService.OnUserChanged += RefreshName;
            RefreshName();
        }

        private void OnDestroy() =>
            clientUserService.OnUserChanged -= RefreshName;

        private void RefreshName() =>
            username.text = clientUserService.CurrentUser?.Username;
    }
}