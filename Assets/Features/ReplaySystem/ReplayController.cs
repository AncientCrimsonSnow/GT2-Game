using System;
using System.Collections.Generic;
using System.Linq;
using Features.ReplaySystem.Record;
using UnityEngine;

namespace Features.ReplaySystem
{
    public class ReplayController
    {
        public GameObject OriginatorGameObject { get; private set; }
        public bool IsRecording { get; private set; }
        
        private readonly ReplayManager _replayManager;
        private readonly List<IReplayOriginator> _replayOriginators;
        private readonly List<IInputSnapshot> _ticks;
        
        private int _registrationTick;
        private int _currentIndex;
        private readonly Action _onCompleteReplay;
        private bool _isLoop;
        private bool _stopNextTick;
        
        public ReplayController(ReplayManager replayManager, GameObject originatorGroup, Action onCompleteReplay)
        {
            OriginatorGameObject = originatorGroup;
            _replayManager = replayManager;
            _onCompleteReplay = onCompleteReplay;
            _replayOriginators = new List<IReplayOriginator>();
            _ticks = new List<IInputSnapshot>();
            IsRecording = true;
        }
        
        public void RegisterOriginator(IReplayOriginator replayOriginator)
        {
            replayOriginator.PushNewTick += PushNewTick;
            
            _replayOriginators.Add(replayOriginator);
        }

        public void UnregisterOriginator(IReplayOriginator replayOriginator)
        {
            foreach (var originator in _replayOriginators.Where(originator => originator == replayOriginator))
            {
                originator.PushNewTick -= PushNewTick;
            }

            _replayOriginators.Remove(replayOriginator);
        }

        public void StartReplay(bool isLoop)
        {
            IsRecording = false;
            _isLoop = isLoop;
        }

        public void Tick()
        {
            if (_stopNextTick || _ticks.Count == 0)
            {
                StopReplay();
                return;
            }
            
            _ticks[_currentIndex].Tick(_replayManager.TickDurationInSeconds);
            _currentIndex++;

            if (_currentIndex < _ticks.Count) return;
            if (_isLoop)
            {
                _currentIndex = 0;
            }
            else
            {
                _stopNextTick = true;
            }
        }

        public void StopReplay()
        {
            _onCompleteReplay.Invoke();
        }
        
        private void PushNewTick(IInputSnapshot inputSnapshot)
        {
            _replayManager.Tick();
            inputSnapshot.Tick(_replayManager.TickDurationInSeconds);
            _ticks.Add(inputSnapshot);
        }
    }
}
