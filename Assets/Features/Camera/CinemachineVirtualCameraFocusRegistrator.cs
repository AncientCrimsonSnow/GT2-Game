using Cinemachine;
using UnityEngine;

namespace Features.Camera
{
    public class CinemachineVirtualCameraFocusRegistrator : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private UpdateInteractionText canvas;
        [SerializeField] private GameObject entryCaster;

        private void Awake()
        {
            cinemachineVirtualCameraFocus.SetFocus(cinemachineVirtualCamera);
            UpdateInteractionText interactionText = Instantiate(canvas, cinemachineVirtualCamera.Follow);
            cinemachineVirtualCameraFocus.InitCanvas(interactionText, entryCaster);
            cinemachineVirtualCameraFocus.SetFollow(cinemachineVirtualCamera.Follow).SetCurrentFollowAsRestore();
        }
    }
}
