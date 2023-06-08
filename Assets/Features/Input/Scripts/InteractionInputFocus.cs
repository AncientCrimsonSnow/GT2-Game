using Features;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InteractionInputFocus", menuName = "Focus/Input/Interaction")]
public class InteractionInputFocus : Focus_SO<BaseInteractionInput>
{
    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        Focus.OnInteractionInput(context);
    }
}
