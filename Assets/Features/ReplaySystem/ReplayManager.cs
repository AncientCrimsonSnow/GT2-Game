using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    //TODO: replayables must be differentiated between own behaviour, own behaviour + record, replay
    //TODO: make replayables be able to unregister itself -> do this, when some systems need it
    
    //TODO: method for replayables, when the state switches
    
    /// <summary>
    /// 
    /// </summary>
    public class ReplayManager : MonoBehaviour
    {
        public static ReplayManager Instance { get; private set; }

        public float AdvancedTime => _advancedTime;
        
        public int AdvancedTicks { get; private set; }

        private bool HasCurrentReplayController => _replayControllerList.Count > 0;
        private ReplayController CurrentReplayController => _replayControllerList[^1];
        
        private float _advancedTime;
        private readonly List<ReplayController> _replayControllerList = new List<ReplayController>();
        
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        [SerializeField] private float tickDurationInSeconds;

        private bool _tickPerformed;
        private float _tickTimeDelta;

        private int _currentTick;

        private void Update()
        {
            if (!_tickPerformed) return;
            
            if (_tickTimeDelta > tickDurationInSeconds)
            {
                _tickPerformed = false;
            }
            else
            {
                _tickTimeDelta += Time.deltaTime;
            }
        }

        private void Tick()
        {
            if (_tickPerformed) return;

            foreach (var replayController in _replayControllerList)
            {
                replayController.Tick(tickDurationInSeconds);
            }

            _tickPerformed = true;
            _tickTimeDelta = 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="replayable"></param>
        public void InitializeRecord(IReplayable replayable)
        {
            //TODO: implement action as condition, if tick is valid
            var replayController = new ReplayController(this, replayable);
            _replayControllerList.Add(replayController);
        }

        public void StopRecording(bool isLoop)
        {
            CurrentReplayController.StopRecording(isLoop);
        }

        public void UnregisterReplayable()
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="replayable"></param>
        public void UnregisterReplayable(IReplayable replayable)
        {
            if (!_registeredReplayables.Remove(replayable))
            {
                Debug.LogError("Removing of a replayable inside the ReplayManager failed! Maybe it was not found inside the collection!");
            }

            foreach (var replayController in _replayControllerList)
            {
                replayController.UnregisterReplayable(replayable);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="reverseTimeScale"></param>
        /// <param name="forwardTimeScale"></param>
        public void StartReplayRotation(float duration, float reverseTimeScale = 3f, float forwardTimeScale = 1f)
        {
            if (HasCurrentReplayController && CurrentReplayController.IsRecording(typeof(RecordSequenceState), typeof(ReverseReplaySequenceState)))
            {
                Debug.LogWarning("Cant create a new ReplayController while inside the ReplaySequenceState or ReverseReplayState");
                return;
            }

            var newReplayController = new ReplayController(_replayControllerList, _advancedTime, 
                duration, reverseTimeScale, forwardTimeScale);
            newReplayController.RegisterMultipleReplayables(_registeredReplayables);
            _replayControllerList.Add(newReplayController);
        }

        /// <summary>
        /// Checks if the ReplayManager is i a certain state.
        /// </summary>
        /// <param name="replaySequenceStates">The requested states. If none is passed, it will look, if it has any state</param>
        /// <returns>Result, if the requested state was found</returns>
        public bool IsInState(params Type[] replaySequenceStates)
        {
            return HasCurrentReplayController && CurrentReplayController.IsRecording(replaySequenceStates);
        }

        /// <summary>
        /// Directly ends the recording state of the replay system. It is based on the advanced time since the StartReplayRotation method call.
        /// Will get skipped, if the replay system isn't in the record state or there is no ongoing replay at all.
        /// </summary>
        public void FastForwardRecord()
        {
            if (!HasCurrentReplayController && !CurrentReplayController.IsRecording(typeof(RecordSequenceState))) return;

            CurrentReplayController.OverrideEndingTime(_advancedTime);
        }
    }
}