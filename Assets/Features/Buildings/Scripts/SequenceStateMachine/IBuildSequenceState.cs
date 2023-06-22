using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IBuildSequenceState
{
    public bool TryGetNext(out IBuildSequenceState nextState);

    public bool TryGetPrevious(out IBuildSequenceState nextState);

    public void OnPerform(InputAction.CallbackContext context);
}
