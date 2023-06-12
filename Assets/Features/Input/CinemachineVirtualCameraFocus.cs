using Cinemachine;
using Features;
using UnityEngine;

[CreateAssetMenu(fileName = "CinemachineVirtualCameraFocus", menuName = "Focus/CinemachineVirtualCamera")]
public class CinemachineVirtualCameraFocus : Focus_SO<CinemachineVirtualCamera>
{
    private Transform _restoreTransform;
    private Transform _currentFollow;

    public bool HasCurrentFollow()
    {
        return _currentFollow != null;
    }

    public Transform GetRestoreFollow()
    {
        return _restoreTransform;
    }
    
    public void SetFollow(Transform follow)
    {
        _currentFollow = follow;
        Focus.Follow = follow;
        Focus.LookAt = follow;
    }
    
    public void SetCurrentFollowAsRestore()
    {
        _restoreTransform = _currentFollow;
    }
}
