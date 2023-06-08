using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.TileSystem.CharacterBehaviours
{
    public abstract class BaseMagicInput : MonoBehaviour
    {
        public abstract void OnMagicInput(InputAction.CallbackContext context);
    }
}