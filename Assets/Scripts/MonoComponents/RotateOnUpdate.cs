using UnityEngine;

namespace TEDinc.PhotosNetwork.MonoComponents
{
    public class RotateOnUpdate : MonoBehaviour
    {
        [SerializeField]
        private float anglePerSecond = 45f;
        private float angle;
        void Update()
        {
            angle += anglePerSecond * Time.deltaTime;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}