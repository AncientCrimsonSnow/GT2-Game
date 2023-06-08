using System;
using DG.Tweening;
using Features.TileSystem.TileSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Ease easeType;
    [SerializeField] private float movementSpeed;
    
    private Animator _characterAnimator;
    private Vector2 _storedInputVector;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveZ = Animator.StringToHash("MoveZ");
    private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveZ = Animator.StringToHash("LastMoveZ");

    private void Awake()
    {
        //_characterAnimator = GetComponent<Animator>();
    }
    
    public void SetMovementInput(InputAction.CallbackContext context)
    {
        _storedInputVector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (DOTween.IsTweening(transform)) return;
        var inputMovement = new Vector3(_storedInputVector.x, 0, _storedInputVector.y);
        TweenMove(inputMovement);
        //SetMovementAnimation(inputVector);
    }

    private void TweenMove(Vector3 inputVector)
    {
        var position = TileHelper.TransformPositionToVector3(transform);
        var nextPosition = position + inputVector;
        float distance = Vector3.Distance(position, nextPosition);
        float movementTime = distance / movementSpeed;
        transform.DOMove(nextPosition, movementTime).SetEase(easeType);
    }
    
    private void SetMovementAnimation(Vector2 inputVector)
    {
        //Set animation to movement
        _characterAnimator.SetFloat(MoveX, inputVector.x);
        _characterAnimator.SetFloat(MoveZ, inputVector.y);

        //Set the correct idle animation
        if (inputVector is { x: 0, y: 0 }) return;
        _characterAnimator.SetFloat(LastMoveX, inputVector.x);
        _characterAnimator.SetFloat(LastMoveZ, inputVector.y);
    }
}
