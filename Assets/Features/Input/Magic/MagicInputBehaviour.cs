using Cinemachine;
using Features.Input;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicInputBehaviour : BaseMagicInput
{
    //TODO: swap magic input focus during skeleton steering
    
    [SerializeField] private bool breakAutoTicksEntry;

    [SerializeField] private TileManager tileManager;
    [SerializeField] private SkeletonFocus skeletonFocus;
    [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private BaseMovementInput _entryMovementInput;
    private bool _breakAutoTick;

    #region figure this out
    private void Awake()
    {
        _breakAutoTick = breakAutoTicksEntry;
    }

    private void Start()
    {
        cinemachineVirtualCameraFocus.SetFocus(virtualCamera);
        cinemachineVirtualCameraFocus.SetFollow(transform);
        cinemachineVirtualCameraFocus.SetCurrentFollowAsRestore();
    }

    private void Update()
    {
        if (skeletonFocus.ContainsFocus() || _breakAutoTick) return;
        
        ReplayManager.Instance.Tick();
    }
    #endregion

    public override void OnMagicInput(InputAction.CallbackContext context)
    {
        tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform)).TryCastMagic(gameObject);
    }
    
    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        _breakAutoTick = !_breakAutoTick;
    }
}
