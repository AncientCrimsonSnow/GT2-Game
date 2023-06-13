using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    public abstract class BaseMagicInput : MonoBehaviour
    {
        public abstract void OnMagicInput(InputAction.CallbackContext context);

        public abstract void OnInterruptMagic(InputAction.CallbackContext context);
    }
}