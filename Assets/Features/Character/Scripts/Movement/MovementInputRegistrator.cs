using UnityEngine;

namespace Features.Character.Scripts.Movement
{
    public class MovementInputRegistrator : MonoBehaviour
    {
        [Header("Topic")]
        [SerializeField] private DirectionInputFocus directionInputFocus;
        [SerializeField] private BaseMovementInput movementInputBehaviour;
        [SerializeField] private bool setAsRestore;

        [Header("RegistrationTime")] 
        [SerializeField] private bool awake;
        [SerializeField] private bool start;

        private void Awake()
        {
            if (!awake) return;
            
            directionInputFocus.SetFocus(movementInputBehaviour);
            if (setAsRestore)
            {
                directionInputFocus.SetCurrentAsRestore();
            }
        }

        private void Start()
        {
            if (!start) return;
            
            directionInputFocus.SetFocus(movementInputBehaviour);
            if (setAsRestore)
            {
                directionInputFocus.SetCurrentAsRestore();
            }
        }
    }
}
