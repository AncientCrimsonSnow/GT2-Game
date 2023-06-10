using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseInteractionInput : MonoBehaviour
{
    public abstract void OnInteractionInput(InputAction.CallbackContext context);
}
