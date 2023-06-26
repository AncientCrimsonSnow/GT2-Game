using Cinemachine;
using UnityEngine;

namespace Features.Camera
{
    public class CinemachineVirtualCameraFocusRegistrator : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Awake()
        {
            cinemachineVirtualCameraFocus.SetFocus(cinemachineVirtualCamera);
            cinemachineVirtualCameraFocus.SetFollow(cinemachineVirtualCamera.Follow).SetCurrentFollowAsRestore();
        }
    }
}
