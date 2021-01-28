using System.Collections;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class LoggedInUserManager : ClientServiceInteractorBase
    {
        [SerializeField]
        private TMP_Text username;

        private IClientUserService clientUserService;

        protected override void OnInit()
        {
            clientUserService = clientRunner.GetService<IClientUserService>(); 
            clientUserService.OnUserChanged += RefreshName;
            RefreshName();
        }

        private void OnDestroy() =>
            clientUserService.OnUserChanged -= RefreshName;

        private void RefreshName() =>
            username.text = clientUserService.CurrentUser?.Username;

        public void LogOut() =>
            clientUserService.LogOut();
    }
}