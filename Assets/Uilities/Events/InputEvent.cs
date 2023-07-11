using UnityEngine;
using UnityEngine.InputSystem;

namespace Uilities.Events
{
    [CreateAssetMenu(fileName = "InputEvent", menuName = "Data/Events/InputEvent")]
    public class InputEvent : ParametrizedGameEvent<InputAction.CallbackContext>
    {

    }
}