using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DataStructures.Events
{
    [CreateAssetMenu(fileName = "InputEvent", menuName = "Data/Events/InputEvent")]
    public class InputEvent : ParametrizedGameEvent<InputAction.CallbackContext>
    {

    }
}