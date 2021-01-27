using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class PublicationLoadController : MonoBehaviour
    {
        [SerializeField]
        private ClientRunner clientRunner;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private RectTransform publicationsContent;
        [Header("UpdateSettings")]
        [SerializeField, Min(0.1f)]
        private float refreshUpdateDelay = 1f;
        [SerializeField]
        private float distanceToUpdate = 400f;

        private IClientPublicationService publicationService;
        private float lastRefreshTime;


        private IEnumerator Start()
        {
            while (!clientRunner.Initilized)
                yield return new WaitForSeconds(0.1f);

            publicationService = clientRunner.GetService<IClientPublicationService>();
            scrollRect.onValueChanged.AddListener(OnScroll);
        }


        private void OnScroll(Vector2 scrollDelta)
        {
            if (lastRefreshTime + refreshUpdateDelay < Time.timeSinceLevelLoad)
            {
                float distnceFromBottom = scrollDelta.y * publicationsContent.sizeDelta.y;
                float distanceFromTop = publicationsContent.sizeDelta.y - distnceFromBottom;
                bool updateFromTop = distanceFromTop < distanceToUpdate;
                bool updateFromBottom = distnceFromBottom < distanceToUpdate;

                if (updateFromTop || updateFromBottom)
                {
                    lastRefreshTime = Time.timeSinceLevelLoad;
                    publicationService.Load(updateFromBottom ? GetDataMode.Before : GetDataMode.After);
                }
            }
        }
    }
}