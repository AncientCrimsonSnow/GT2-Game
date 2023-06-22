using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface ICastInput
{
    void OnCastInput(InputAction.CallbackContext context);

    void OnInterruptCast(InputAction.CallbackContext context);
}
