using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.TileSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractBehaviour : MonoBehaviour
{
    private ITileManager _tileManager;
    
    [Inject]
    public void Initialize(ITileManager tileManager)
    {
        _tileManager = tileManager;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var registeredPosition = TileHelper.TransformPositionToInt2(transform);
        var tile = _tileManager.GetTileTypeAt(registeredPosition);
        tile.TryInteract(gameObject);
    }
}
