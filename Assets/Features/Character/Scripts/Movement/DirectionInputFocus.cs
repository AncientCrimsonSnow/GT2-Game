using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    [CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
    public class DirectionInputFocus : RestorableFocus_SO<IDirectionInput>
    {
        public override void SetFocus(IDirectionInput newFocus)
        {
            Focus?.OnDirectionInputFocusChanges();

            base.SetFocus(newFocus);
        }

        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            Focus.OnDirectionInput(context);
        }
    }
}
