using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Interaction
{
    [CreateAssetMenu(fileName = "InteractionInputFocus", menuName = "Focus/Input/Interaction")]
    public class InteractionInputFocus : StackFocus_SO<IInteractInput>
    {
        public void OnInteractionInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
        
            if (!ContainsFocus()) return;
        
            Focus.OnInteractionInput(context);
        }
    }
}
