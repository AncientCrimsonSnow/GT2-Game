using System;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionInputBehaviour : BaseInteractionInput, IReplayOriginator
{
    public Action<IInputSnapshot> PushNewTick { get; set; }
    
    [SerializeField] private TileManager tileManager;

    private void Start()
    {
        ReplayManager.Instance.RegisterOriginator(gameObject, this);
    }

    public override void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (ReplayManager.Instance.IsTickPerformed) return;
        
        PushNewTick.Invoke(new InteractionInputSnapshot(gameObject, tileManager));
    }
}
