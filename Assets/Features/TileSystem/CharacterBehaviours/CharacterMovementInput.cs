using DG.Tweening;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementInput : BaseMovementInput
{
    [SerializeField] private Ease easeType;
    
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
    
    public override void OnMovementInput(InputAction.CallbackContext context)
    {
        _storedInputVector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (DOTween.IsTweening(transform) || _storedInputVector == Vector2.zero) return;
        var inputMovement = new Vector3(_storedInputVector.x, 0, _storedInputVector.y);
        TweenMove(inputMovement);
        //SetMovementAnimation(inputVector);
    }

    private void TweenMove(Vector3 inputVector)
    {
        var position = TileHelper.TransformPositionToVector3(transform);
        Debug.Log("TweenMove " + position + " " + position + inputVector);
        transform.DOMove(position + inputVector, ReplayManager.Instance.TickDurationInSeconds).SetEase(easeType);
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
