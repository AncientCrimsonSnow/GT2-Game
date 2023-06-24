using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSequenceStateMachine
{
    private readonly Action _onSequenceComplete;
    private readonly Action _onCancelSequence;
    private IBuildSequenceState _currentState;
    private bool _sequenceComplete;

    public BuildSequenceStateMachine(IBuildSequenceState entrySequenceState, Action onSequenceComplete, Action onCancelSequence)
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
    }

    public void NextState()
    {
        if (_sequenceComplete) return;

        if (_currentState.TryGetNext(out var nextSequenceState))
        {
            Debug.Log(nextSequenceState);
            _currentState = nextSequenceState;
        }
        else
        {
            _onSequenceComplete?.Invoke();
            _sequenceComplete = true;
        }
    }

    public void Cancel()
    {
        if (_sequenceComplete) return;
        
        _onCancelSequence?.Invoke();
        _sequenceComplete = true;
    }

    public void Perform(InputAction.CallbackContext context)
    {
        if (_sequenceComplete) return;
        
        _currentState.OnPerform(context);
    }
}
