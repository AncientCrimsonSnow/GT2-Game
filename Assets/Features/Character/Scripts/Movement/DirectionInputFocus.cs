using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    [CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
    public class DirectionInputFocus : RestorableFocus_SO<IDirectionInput>
    {
        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            Focus.OnDirectionInput(context);
        }
    }
}
