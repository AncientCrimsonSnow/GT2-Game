using UnityEngine.InputSystem;

namespace Features
{
    public interface ICastInput
    {
        void OnCastInput(InputAction.CallbackContext context);

        void OnInterruptCast(InputAction.CallbackContext context);
    }
}
