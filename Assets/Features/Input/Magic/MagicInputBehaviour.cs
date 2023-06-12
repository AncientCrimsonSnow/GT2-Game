using System;
using Cinemachine;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicInputBehaviour : BaseMagicInput
{
    [SerializeField] private bool breakAutoTicksEntry;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    
    [SerializeField] private GameObject magicInstantiationPrefab;

    private GameObject _newestInstantiatedSkeleton;
    private BaseMovementInput _entryMovementInput;
    private Transform _entryCameraFollow;
    private bool _breakAutoTick;

    private void Awake()
    {
        _breakAutoTick = breakAutoTicksEntry;
    }

    private void Update()
    {
        if (_newestInstantiatedSkeleton != null || _breakAutoTick) return;
        
        ReplayManager.Instance.Tick();
    }

    public override void OnMagicInput(InputAction.CallbackContext context)
    {
        if (_newestInstantiatedSkeleton == null)
        {
            FocusSkeleton();
        }
        else
        {
            SetLoop();
            ResetFocus();
        }
    }
    
    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        if (_newestInstantiatedSkeleton == null)
        {
            _breakAutoTick = !_breakAutoTick;
        }
        else
        {
            ReplayManager.Instance.StopReplayable(_newestInstantiatedSkeleton);
        }
    }

    private void FocusSkeleton()
    {
        _entryMovementInput = movementInputFocus.GetFocus();
        _newestInstantiatedSkeleton = Instantiate(magicInstantiationPrefab, transform.position, Quaternion.identity);

        //the newestInstantiatedSkeleton must be buffered, because it will be null during replay
        var bufferInstantiation = _newestInstantiatedSkeleton;
        ReplayManager.Instance.InitializeRecording(_newestInstantiatedSkeleton, () =>
        {
            //destroy, if a record gets interrupted
            if (_newestInstantiatedSkeleton != null)
            {
                ResetFocus();
            }
            
            ReplayManager.Instance.UnregisterReplayable(bufferInstantiation);
            Destroy(bufferInstantiation);
        });

        _entryCameraFollow = virtualCamera.Follow;
        SetCameraFollow(_newestInstantiatedSkeleton.transform);

        if (_newestInstantiatedSkeleton.TryGetComponent(out BaseMovementInput baseMovementInput))
        {
            movementInputFocus.SetFocus(baseMovementInput);
        }

        if (_newestInstantiatedSkeleton.TryGetComponent(out BaseInteractionInput baseInteractionInput))
        {
            interactionInputFocus.SetFocus(baseInteractionInput);
        }
    }
    
    private void SetLoop()
    {
        var isLoop = TileHelper.TransformPositionToVector3Int(_newestInstantiatedSkeleton.transform) == TileHelper.TransformPositionToVector3Int(transform);
        ReplayManager.Instance.StartReplay(_newestInstantiatedSkeleton, isLoop);
    }

    private void ResetFocus()
    {
        movementInputFocus.SetFocus(_entryMovementInput);
        interactionInputFocus.Restore();

        SetCameraFollow(_entryCameraFollow);
            
        _newestInstantiatedSkeleton = null;
    }
    
    private void SetCameraFollow(Transform newTransform)
    {
        virtualCamera.Follow = newTransform;
        virtualCamera.LookAt = newTransform;
    }
}
