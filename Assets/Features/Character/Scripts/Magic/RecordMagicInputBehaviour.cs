using Features.Input;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecordMagicInputBehaviour : BaseMagicInput
{
    [Header("Character Focus")]
    [SerializeField] private SkeletonFocus skeletonFocus;
    [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    
    [Header("Input Focus")]
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    [SerializeField] private MagicInputFocus magicInputFocus;

    private Vector3Int _originPosition;
    
    private void Awake()
    {
        _originPosition = TileHelper.TransformPositionToVector3Int(transform);
    }

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
        var isLoop = _originPosition == TileHelper.TransformPositionToVector3Int(transform);
        ReplayManager.Instance.StartReplay(skeletonFocus.GetFocus(), isLoop);
    }

    //TODO: duplicate code
    private void ResetFocus()
    {
        movementInputFocus.Restore();
        interactionInputFocus.Restore();
        magicInputFocus.Restore();
        cinemachineVirtualCameraFocus.RestoreFollow();
        skeletonFocus.Restore();
    }
}
