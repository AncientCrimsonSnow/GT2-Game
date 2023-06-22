using Uilities.Focus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    [CreateAssetMenu(fileName = "MagicInputFocus", menuName = "Focus/Input/Magic")]
    public class CastInputFocus : RestorableFocus_SO<ICastInput>
    {
        public void OnCastInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
        
            Focus.OnCastInput(context);
        }

        public void OnInterruptCast(InputAction.CallbackContext context)
        {
            if (!context.started) return;
        
            Focus.OnInterruptCast(context);
        }
    }
}
