using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Interaction
{
    public abstract class BaseInteractionInput : MonoBehaviour
    {
        public abstract void OnInteractionInput(InputAction.CallbackContext context);
    }
}
