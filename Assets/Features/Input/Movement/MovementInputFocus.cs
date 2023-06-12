using Features;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MovementInputFocus", menuName = "Focus/Input/Movement")]
public class MovementInputFocus : Focus_SO<BaseMovementInput>
{
    private BaseMovementInput _restoreValue;
    
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Focus.OnMovementInput(context);
    }

    public override void Restore()
    {
        if (_restoreValue != null)
        {
            SetFocus(_restoreValue);
        }
        else
        {
            base.Restore();
        }
    }

    public void SetCurrentAsRestore()
    {
        _restoreValue = Focus;
    }
}
