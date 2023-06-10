using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;

public class MovementInputSnapshot : IInputSnapshot
{
    private readonly Transform _transform;
    private readonly Animator _animator;
    private readonly Vector2 _storedInputVector;
    private readonly Ease _easeType;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveZ = Animator.StringToHash("MoveZ");
    private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveZ = Animator.StringToHash("LastMoveZ");

    public MovementInputSnapshot(Transform transform, Vector2 storedInputVector, Ease easeType)
    {
        _transform = transform;
        _storedInputVector = storedInputVector;
        _easeType = easeType;
    }
    
    public void Tick(float tickDurationInSeconds)
    {
        TweenMove(tickDurationInSeconds);
        //SetMovementAnimation();
    }
    
    private void TweenMove(float tickDurationInSeconds)
    {
        var inputMovement = new Vector3(_storedInputVector.x, 0, _storedInputVector.y);
        var position = TileHelper.TransformPositionToVector3Int(_transform);
        _transform.DOMove(position + inputMovement, tickDurationInSeconds).SetEase(_easeType);
    }
    
    private void SetMovementAnimation()
    {
        //Set animation to movement
        _animator.SetFloat(MoveX, _storedInputVector.x);
        _animator.SetFloat(MoveZ, _storedInputVector.y);

        //Set the correct idle animation
        if (_storedInputVector is { x: 0, y: 0 }) return;
        _animator.SetFloat(LastMoveX, _storedInputVector.x);
        _animator.SetFloat(LastMoveZ, _storedInputVector.y);
    }
}
