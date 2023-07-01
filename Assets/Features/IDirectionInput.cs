using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDirectionInput
{
    void OnDirectionInputFocusChanges();
    
    void OnDirectionInput(InputAction.CallbackContext context);
}
