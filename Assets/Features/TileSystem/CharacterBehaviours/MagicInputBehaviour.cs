using Cinemachine;
using Features.TileSystem.CharacterBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicInputBehaviour : BaseMagicInput
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    
    [SerializeField] private GameObject magicInstantiationPrefab;

    private GameObject _newestInstantiatedSkeleton;
    private BaseMovementInput _entryMovementInput;

    public override void OnMagicInput(InputAction.CallbackContext context)
    {
        if (_newestInstantiatedSkeleton == null)
        {
            //TODO: initialize Recording
            
            _entryMovementInput = movementInputFocus.GetFocus();
            _newestInstantiatedSkeleton = Instantiate(magicInstantiationPrefab, transform.position, Quaternion.identity);
            
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
        else
        {
            //TODO: ReplayManager.Instance.StartReplay() -> it loops, when skeleton comes back to origin position, otherwise not
            
            movementInputFocus.SetFocus(_entryMovementInput);
            interactionInputFocus.Restore();

            SetCameraFollow(transform);
            
            _newestInstantiatedSkeleton = null;
        }
    }
    
    public override void OnInterruptMagic(InputAction.CallbackContext context)
    {
        if (_newestInstantiatedSkeleton == null) return;
        
        movementInputFocus.SetFocus(_entryMovementInput);
        interactionInputFocus.Restore();
        Destroy(_newestInstantiatedSkeleton);
    }
    
    private void SetCameraFollow(Transform newTransform)
    {
        virtualCamera.Follow = newTransform;
        virtualCamera.LookAt = newTransform;
    }
}
