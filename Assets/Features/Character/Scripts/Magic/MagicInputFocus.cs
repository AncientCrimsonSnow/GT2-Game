using Features;
using Features.TileSystem.CharacterBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MagicInputFocus", menuName = "Focus/Input/Magic")]
public class MagicInputFocus : RestorableFocus_SO<BaseMagicInput>
{
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
}
