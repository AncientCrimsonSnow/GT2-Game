using System;
using NewReplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseMovementInput : MonoBehaviour
{
    public abstract void OnMovementInput(InputAction.CallbackContext context);
}
