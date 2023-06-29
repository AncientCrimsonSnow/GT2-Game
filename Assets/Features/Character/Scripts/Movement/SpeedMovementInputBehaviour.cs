using DG.Tweening;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    public class SpeedMovementInputBehaviour : BaseMovementInput
    {
        [SerializeField] private Ease easeType;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Animator animator;
    
        private Vector2 _storedInputVector;
    
        //TODO: duplicate
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveZ = Animator.StringToHash("MoveZ");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveZ = Animator.StringToHash("LastMoveZ");


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void OnDirectionInput(InputAction.CallbackContext context)
        {
            _storedInputVector = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (DOTween.IsTweening(transform) || _storedInputVector == Vector2.zero) return;

            TweenMove();
            //SetMovementAnimation();
        }
    
        private void TweenMove()
        {
            var inputMovement = new Vector3(_storedInputVector.x, 0, _storedInputVector.y);
            var position = TileHelper.TransformPositionToVector3Int(transform);
            var distance = Vector3.Distance(position, position + inputMovement);
            var movementTime = distance / movementSpeed;
            transform.DOMove(position + inputMovement, movementTime).SetEase(easeType);

            transform.rotation =
                Quaternion.LookRotation(new Vector3(_storedInputVector.x, transform.position.y, _storedInputVector.y));
        }
    
        private void SetMovementAnimation()
        {
            //Set animation to movement
            animator.SetFloat(MoveX, _storedInputVector.x);
            animator.SetFloat(MoveZ, _storedInputVector.y);

            //Set the correct idle animation
            if (_storedInputVector is { x: 0, y: 0 }) return;
            animator.SetFloat(LastMoveX, _storedInputVector.x);
            animator.SetFloat(LastMoveZ, _storedInputVector.y);
        }
    }
}
