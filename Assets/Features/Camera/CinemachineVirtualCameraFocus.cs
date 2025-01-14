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
        private UpdateInteractionText _interactionText;
        private GameObject _defaultCaster;

        public void InitCanvas(UpdateInteractionText canvas, GameObject defaultCaster)
        {
            _interactionText = canvas;
            _defaultCaster = defaultCaster;
            _interactionText.SetCaster(defaultCaster);
        }
        
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
        
            _interactionText.transform.SetParent(_currentFollow);
            _interactionText.SetCaster(_currentFollow.gameObject);
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
        
            _interactionText.transform.SetParent(_restoreTransform);
            _interactionText.SetCaster(_defaultCaster);
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
