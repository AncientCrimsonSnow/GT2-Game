using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    public abstract class BaseMovementInput : MonoBehaviour, IDirectionInput
    {
        public abstract void OnMovementInput(InputAction.CallbackContext context);
    }
}
