using UnityEngine;

namespace VENTUS.Controlling
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The transform, the billboard will Look at. By default it will face towards Camera.Main")]
        private Transform _lookAt;

        [SerializeField] private bool _mirror;

        private Transform _lookAtTransform;

        private void Start()
        {
            if (!_lookAt && Camera.main != null)
            {
                _lookAtTransform = Camera.main.transform;
            }
            else
            {
                _lookAtTransform = _lookAt;
            }
        }

        private void LateUpdate()
        {
            transform.rotation = BillboardUtility.GetRotation(transform.position, _lookAtTransform.position, _mirror);
        }
    }
}
