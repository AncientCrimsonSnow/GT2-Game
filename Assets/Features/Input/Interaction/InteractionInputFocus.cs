using Features;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InteractionInputFocus", menuName = "Focus/Input/Interaction")]
public class InteractionInputFocus : Focus_SO<BaseInteractionInput>
{
    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        if (!ContainsFocus()) return;
        
        Focus.OnInteractionInput(context);
    }
}
