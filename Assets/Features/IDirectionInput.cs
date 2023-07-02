using UnityEngine.InputSystem;

namespace Features
{
    public interface IDirectionInput
    {
        void OnDirectionInputFocusChanges();
    
        void OnDirectionInput(InputAction.CallbackContext context);
    }
}
