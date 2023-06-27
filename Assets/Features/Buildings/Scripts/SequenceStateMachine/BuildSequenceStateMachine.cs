using System;
using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSequenceStateMachine
{
    private readonly Action<BuildData> _onSequenceComplete;
    private readonly Action _onCancelSequence;
    private IBuildSequenceState _currentState;
    private bool _sequenceComplete;

    public BuildSequenceStateMachine(IBuildSequenceState entrySequenceState, Action<BuildData> onSequenceComplete, Action onCancelSequence)
    {
        _currentState = entrySequenceState;
        _onSequenceComplete = onSequenceComplete;
        _onCancelSequence = onCancelSequence;
    }

    public void PreviousState()
    {
        if (_sequenceComplete) return;
        
        if (_currentState.TryGetPrevious(out var previousSequenceState))
        {
            Debug.Log(previousSequenceState);
            _currentState = previousSequenceState;
        }
        else
        {
            Cancel();
        }
    }

    public void NextState()
    {
        if (_sequenceComplete) return;

        if (_currentState.TryCompleteSequence(out var nextSequenceState))
        {
            _onSequenceComplete?.Invoke(_currentState.GetSelectedObject());
            _sequenceComplete = true;
            _currentState = null;
        }
        else
        {
            Debug.Log(nextSequenceState);
            _currentState = nextSequenceState;
        }
    }

    public void Cancel()
    {
        if (_sequenceComplete) return;
        
        _onCancelSequence?.Invoke();
        _sequenceComplete = true;
        _currentState = null;
    }

    public void Perform(InputAction.CallbackContext context)
    {
        if (_sequenceComplete) return;
        
        _currentState.OnPerform(context);
    }
}
