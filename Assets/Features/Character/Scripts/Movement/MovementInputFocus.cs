using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
public class MovementInputFocus : RestorableFocus_SO<BaseMovementInput>
{
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Focus.OnMovementInput(context);
    }
}
