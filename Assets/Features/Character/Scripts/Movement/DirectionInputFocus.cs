using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    [CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
    public class DirectionInputFocus : StackFocus_SO<IDirectionInput>
    {
        public override void PushFocus(IDirectionInput newFocus)
        {
            Focus?.OnDirectionInputFocusChanges();

            base.PushFocus(newFocus);
        }

        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            Focus.OnDirectionInput(context);
        }
    }
}
