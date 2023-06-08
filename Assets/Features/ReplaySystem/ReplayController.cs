using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayController
    {
        public IReplayOriginator ReplayOriginator { get; private set; }
        public bool IsRecording { get; private set; }

        private readonly ReplayManager _replayManager;
        private readonly List<IRecordSnapshot> _ticks;
        
        private int _registrationTick;
        private int _currentIndex;
        private Action _onCompleteReplay;
        private bool _isLoop;
        

        public ReplayController(ReplayManager replayManager, IReplayOriginator replayOriginator)
        {
            ReplayOriginator = replayOriginator;
            _replayManager = replayManager;
            _ticks = new List<IRecordSnapshot>();
            
            ReplayOriginator.ProvideReplaySnapshot += ProvideReplayEventFrame;
            IsRecording = true;
        }

        public void StartReplay(bool isLoop, Action onCompleteReplay)
        {
            ReplayOriginator.ProvideReplaySnapshot -= ProvideReplayEventFrame;
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
            
            _ticks[_currentIndex].Tick();
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
            if (_replayManager.AdvancedTicks == _registrationTick)
            {
                Debug.LogWarning("You can only register one record per tick!");
                return;
            }
            
            _registrationTick = _replayManager.AdvancedTicks;
            _ticks.Add(recordSnapshot);
        }
    }
}
