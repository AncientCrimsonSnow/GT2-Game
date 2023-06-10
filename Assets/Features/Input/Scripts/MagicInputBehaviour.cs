using System;
using Cinemachine;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: if move of skeleton ain't valid -> destroy him
public class MagicInputBehaviour : BaseMagicInput
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    
    [SerializeField] private GameObject magicInstantiationPrefab;

    private GameObject _newestInstantiatedSkeleton;
    private BaseMovementInput _entryMovementInput;
    private Transform _entryCameraFollow;

    private void Update()
    {
        if (_newestInstantiatedSkeleton != null) return;
        
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
            ResetFocus();
        }
    }
    
    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        if (_newestInstantiatedSkeleton == null) return;
        
        movementInputFocus.SetFocus(_entryMovementInput);
        interactionInputFocus.Restore();
        Destroy(_newestInstantiatedSkeleton);
    }

    private void FocusSkeleton()
    {
        _entryMovementInput = movementInputFocus.GetFocus();
        _newestInstantiatedSkeleton = Instantiate(magicInstantiationPrefab, transform.position, Quaternion.identity);
        ReplayManager.Instance.InitializeRecording(_newestInstantiatedSkeleton);

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

    private void ResetFocus()
    {
        var isLoop = TileHelper.TransformPositionToVector3Int(_newestInstantiatedSkeleton.transform) == TileHelper.TransformPositionToVector3Int(transform);
        ReplayManager.Instance.StartReplay(_newestInstantiatedSkeleton, isLoop);
            
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
