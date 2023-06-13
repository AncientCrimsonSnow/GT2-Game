using Cinemachine;
using Uilities.Focus;
using UnityEngine;

namespace Features.Camera
{
    [CreateAssetMenu(fileName = "CinemachineVirtualCameraFocus", menuName = "Focus/CinemachineVirtualCamera")]
    public class CinemachineVirtualCameraFocus : Focus_SO<CinemachineVirtualCamera>
    {
        private Transform _restoreTransform;
        private Transform _currentFollow;

        public CinemachineVirtualCameraFocus SetFollow(Transform follow)
        {
            _currentFollow = follow;
            return this;
        }
    
        public void ApplyFollow()
        {
            if (!ContainsFocus() || _currentFollow == null)
            {
                Debug.LogWarning("Couldn't apply Follow!");
                return;
            }
        
            Focus.Follow = _currentFollow;
            Focus.LookAt = _currentFollow;
        }

        public void RestoreFollow()
        {
            if (!ContainsFocus() || _currentFollow == null)
            {
                Debug.LogWarning("Couldn't apply Follow!");
                return;
            }
        
            Focus.Follow = _restoreTransform;
            Focus.LookAt = _restoreTransform;
        }
    
        public void SetCurrentFollowAsRestore()
        {
            if (_currentFollow == null)
            {
                Debug.LogWarning("Couldn't set current Follow as restore!");
                return;
            }
        
            _restoreTransform = _currentFollow;
        }
    }
}
