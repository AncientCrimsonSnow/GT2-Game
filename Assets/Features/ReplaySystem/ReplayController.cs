using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayController
    {
        public GameObject OriginatorGameObject { get; private set; }
        public bool IsRecording { get; private set; }
        
        private readonly ReplayManager _replayManager;
        private readonly List<IReplayOriginator> _replayOriginators;
        private readonly List<List<IRecordSnapshot>> _ticks;
        
        private int _registrationTick;
        private int _currentIndex;
        private Action _onCompleteReplay;
        private bool _isLoop;
        
        public ReplayController(ReplayManager replayManager, GameObject originatorGroup)
        {
            OriginatorGameObject = originatorGroup;
            _replayManager = replayManager;
            _replayOriginators = new List<IReplayOriginator>();
            _ticks = new List<List<IRecordSnapshot>>();
            IsRecording = true;
        }
        
        public void RegisterOriginator(IReplayOriginator replayOriginator)
        {
            replayOriginator.ProvideReplaySnapshot += ProvideReplayEventFrame;
            
            _replayOriginators.Add(replayOriginator);
        }

        public void UnregisterOriginator(IReplayOriginator replayOriginator)
        {
            foreach (var originator in _replayOriginators.Where(originator => originator == replayOriginator))
            {
                originator.ProvideReplaySnapshot -= ProvideReplayEventFrame;
            }

            _replayOriginators.Remove(replayOriginator);
        }

        public void StartReplay(bool isLoop, Action onCompleteReplay)
        {
            IsRecording = false;
            _onCompleteReplay = onCompleteReplay;
            _isLoop = isLoop;
        }

        public void Tick()
        {
            if (IsRecording)
            {
                Debug.LogWarning("You can only register Records during a replay");
                return;
            }
            
            if (_ticks.Count == 0)
            {
                Debug.LogWarning("There are no ticks registered for replay");
                return;
            }
            
            foreach (var recordSnapshot in _ticks[_currentIndex])
            {
                recordSnapshot.Tick();
            }
            _currentIndex++;

            if (_currentIndex < _ticks.Count) return;
            if (_isLoop)
            {
                _currentIndex = 0;
            }
            else
            {
                StopReplay();
            }
        }

        public void StopReplay()
        {
            _onCompleteReplay.Invoke();
        }
        
        private void ProvideReplayEventFrame(IRecordSnapshot recordSnapshot)
        {
            if (_replayManager.AdvancedTicks != _registrationTick)
            {
                _registrationTick = _replayManager.AdvancedTicks;
                _ticks.Add(new List<IRecordSnapshot>());
            }
            
            _ticks[^1].Add(recordSnapshot);
        }
    }
}
