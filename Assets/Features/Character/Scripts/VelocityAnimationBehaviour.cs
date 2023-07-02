using UnityEngine;

namespace Features.Character.Scripts
{
    public class VelocityAnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator animator;
    
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
    
        private Vector3 _previousPosition;
        private float _previousMagnitude;
    
        void Start()
        {
            _previousPosition = transform.position;
        }

        void Update()
        {
            Vector3 displacement = transform.position - _previousPosition;

            Vector3 velocity = displacement / Time.deltaTime;

            if (velocity.magnitude > 0 && !animator.GetBool(IsMoving))
            {
                animator.SetBool(IsMoving, true);
            }
        
            if (velocity.magnitude == 0 && animator.GetBool(IsMoving) && _previousMagnitude == 0)
            {
                animator.SetBool(IsMoving, false);
            }

            _previousMagnitude = velocity.magnitude;
            _previousPosition = transform.position;
        }
    }
}
