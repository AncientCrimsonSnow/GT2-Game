using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    public abstract class BaseCastInput : MonoBehaviour, ICastInput
    {
        public abstract void OnCastInput(InputAction.CallbackContext context);

        public abstract void OnInterruptCast(InputAction.CallbackContext context);
    }
}