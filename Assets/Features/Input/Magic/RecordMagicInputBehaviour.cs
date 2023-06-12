using Features.Input;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecordMagicInputBehaviour : BaseMagicInput
{
    [SerializeField] private SkeletonFocus skeletonFocus;
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private MagicInputFocus magicInputFocus;
    
    public override void OnMagicInput(InputAction.CallbackContext context)
    {
        SetLoop();
        ResetFocus();
    }

    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        ReplayManager.Instance.StopReplayable(skeletonFocus.GetFocus());
    }
    
    private void SetLoop()
    {
        var isLoop = skeletonFocus.GetOriginPosition() == TileHelper.TransformPositionToVector3Int(transform);
        ReplayManager.Instance.StartReplay(skeletonFocus.GetFocus(), isLoop);
    }

    private void ResetFocus()
    {
        movementInputFocus.Restore();
        interactionInputFocus.Restore();
        magicInputFocus.Restore();
        
        cinemachineVirtualCameraFocus.SetFollow(cinemachineVirtualCameraFocus.GetRestoreFollow());
        
        skeletonFocus.Restore();
    }
}
