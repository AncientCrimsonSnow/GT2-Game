using DG.Tweening;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    public class SpeedMovementInputBehaviour : BaseMovementInput
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Ease easeType;
        [SerializeField] private float movementSpeed;
    
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        
        private Vector2 _storedInputVector;

        public override void OnDirectionInputFocusChanges()
        {
            Debug.Log("o/");
            _storedInputVector = Vector2.zero;
        }

        public override void OnDirectionInput(InputAction.CallbackContext context)
        {
            _storedInputVector = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (DOTween.IsTweening(transform)) return;

            if (_storedInputVector == Vector2.zero)
            {
                DisableMovementAnimation();
                return;
            }
            
            EnableMovementAnimation();
            TweenMove();
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
        
        private void EnableMovementAnimation()
        {
            animator.SetBool(IsMoving, true);
        }

        private void DisableMovementAnimation()
        {
            animator.SetBool(IsMoving, false);
        }
    }
}
