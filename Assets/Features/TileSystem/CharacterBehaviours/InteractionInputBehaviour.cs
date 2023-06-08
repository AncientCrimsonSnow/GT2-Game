using Features.TileSystem.TileSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionInputBehaviour : BaseInteractionInput
{
    [SerializeField] private TileManager tileManager;
    
    public override void OnInteractionInput(InputAction.CallbackContext context)
    {
        var registeredPosition = TileHelper.TransformPositionToInt2(transform);
        var tile = tileManager.GetTileTypeAt(registeredPosition);
        tile.TryInteract(gameObject);
    }
}
