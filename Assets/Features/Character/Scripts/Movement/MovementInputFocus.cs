using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    [CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
    public class MovementInputFocus : RestorableFocus_SO<IDirectionInput>
    {
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            Focus.OnMovementInput(context);
        }
    }
}
