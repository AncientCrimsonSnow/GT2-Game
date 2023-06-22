using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDirectionInput
{
    void OnMovementInput(InputAction.CallbackContext context);
}
