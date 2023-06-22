using UnityEngine;

namespace Features.Character.Scripts.Movement
{
    public class MovementInputRegistrator : MonoBehaviour
    {
        [Header("Topic")]
        [SerializeField] private MovementInputFocus movementInputFocus;
        [SerializeField] private BaseMovementInput movementInputBehaviour;
        [SerializeField] private bool setAsRestore;

        [Header("RegistrationTime")] 
        [SerializeField] private bool awake;
        [SerializeField] private bool start;

        private void Awake()
        {
            if (!awake) return;
            
            movementInputFocus.SetFocus(movementInputBehaviour);
            if (setAsRestore)
            {
                movementInputFocus.SetCurrentAsRestore();
            }
        }

        private void Start()
        {
            if (!start) return;
            
            movementInputFocus.SetFocus(movementInputBehaviour);
            if (setAsRestore)
            {
                movementInputFocus.SetCurrentAsRestore();
            }
        }
    }
}
