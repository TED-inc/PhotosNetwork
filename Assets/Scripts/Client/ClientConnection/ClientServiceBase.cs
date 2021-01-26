using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public abstract class ClientServiceBase : MonoBehaviour
    {
        [SerializeField]
        protected ClientConnection client;
        [SerializeField]
        private GameObject[] items;

        public void ShowPage()
        {
            foreach (GameObject item in items)
                item.SetActive(true);
        }

        public void HidePage()
        {
            foreach (GameObject item in items)
                item.SetActive(false);
        }
    }
}