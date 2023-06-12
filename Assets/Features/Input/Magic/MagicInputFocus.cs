using Features;
using Features.TileSystem.CharacterBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MagicInputFocus", menuName = "Focus/Input/Magic")]
public class MagicInputFocus : Focus_SO<BaseMagicInput>
{
    private BaseMagicInput _restoreValue;
    
    public void OnMagicInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        Focus.OnMagicInput(context);
    }

    public void OnInterruptMagic(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        Focus.OnInterruptMagic(context);
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
