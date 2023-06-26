using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractInput
{
    void OnInteractionInput(InputAction.CallbackContext context);
}
