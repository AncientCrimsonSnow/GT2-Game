using System;
using DG.Tweening;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReplayMovementInputBehaviour : BaseMovementInput, IReplayOriginator
{
    public Action<IInputSnapshot> PushNewTick { get; set; }
    
    [SerializeField] private Ease easeType;
    
    private Vector2 _storedInputVector;

    private void Start()
    {
        ReplayManager.Instance.RegisterOriginator(gameObject, this);
    }

    public override void OnMovementInput(InputAction.CallbackContext context)
    {
        _storedInputVector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (ReplayManager.Instance.IsTickPerformed || _storedInputVector == Vector2.zero) return;
        
        PushNewTick.Invoke(new MovementInputSnapshot(transform, _storedInputVector, easeType));
    }
}
