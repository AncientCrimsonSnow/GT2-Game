using UnityEngine.InputSystem;

namespace Features
{
    public interface IInteractInput
    {
        void OnInteractionInput(InputAction.CallbackContext context);
    }
}
