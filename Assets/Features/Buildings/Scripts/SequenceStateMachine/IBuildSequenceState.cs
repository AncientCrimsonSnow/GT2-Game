using Features.Items.Scripts;
using UnityEngine.InputSystem;

namespace Features.Buildings.Scripts.SequenceStateMachine
{
    public interface IBuildSequenceState
    {
        public bool TryCompleteSequence(out IBuildSequenceState nextState);

        public bool TryGetPrevious(out IBuildSequenceState nextState);

        public void OnPerform(InputAction.CallbackContext context);

        public BuildData GetSelectedObject();
    }
}
