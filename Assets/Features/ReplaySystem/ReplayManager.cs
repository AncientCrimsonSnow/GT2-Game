using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.ReplaySystem
{
    public class ReplayManager : MonoBehaviour
    {
        [SerializeField] private float tickDurationInSeconds;

        public float TickDurationInSeconds => tickDurationInSeconds;
        public bool IsTickPerformed => _tickPerformed;
        public int AdvancedTicks { get; private set; }

        private readonly List<ReplayController> _replayControllerList = new List<ReplayController>();
        private bool _tickPerformed;
        private float _tickTimeDelta;
        
        private bool HasCurrentReplayController => _replayControllerList.Count > 0;
        private ReplayController CurrentReplayController => _replayControllerList[^1];
        
        #region Singleton
        
        public static ReplayManager Instance { get; private set; }
        
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

        #endregion
        
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

        public void Tick()
        {
            if (_tickPerformed) return;
            
            _tickPerformed = true;
            _tickTimeDelta = 0;

            for (var index = _replayControllerList.Count - 1; index >= 0; index--)
            {
                var replayController = _replayControllerList[index];
                if (replayController.IsRecording) continue;
                replayController.Tick();
            }
            AdvancedTicks++;
        }

        public void InitializeRecording(GameObject originatorGameObject, Action onCompleteAction)
        {
            if (_replayControllerList.Any(replayController => replayController.IsRecording))
            {
                Debug.LogError("You can only initialize a new record, when there is no other object currently recording!");
                return;
            }
            
            if (_replayControllerList.Any(replayController => replayController.OriginatorGameObject == originatorGameObject))
            {
                Debug.LogError("A replayable GameObject can only be initialized once!");
                return;
            }
            
            var replayController = new ReplayController(this, originatorGameObject, onCompleteAction.Invoke);
            _replayControllerList.Add(replayController);
        }
        
        public void RegisterOriginator(GameObject originatorGameObject, IReplayOriginator replayOriginator)
        {
            foreach (var replayController in _replayControllerList
                .Where(replayController => replayController.OriginatorGameObject == originatorGameObject))
            {
                replayController.RegisterOriginator(replayOriginator);
                return;
            }
        }

        public void UnregisterOriginator(GameObject originatorGameObject, IReplayOriginator replayOriginator)
        {
            foreach (var replayController in 
                _replayControllerList.Where(replayController => replayController.OriginatorGameObject == originatorGameObject))
            {
                replayController.UnregisterOriginator(replayOriginator);
                return;
            }
        }

        public void StartReplay(GameObject originatorGameObject, bool isLoop)
        {
            if (!HasCurrentReplayController)
            {
                Debug.LogError("There is currently no replayable registered!");
                return;
            }

            if (CurrentReplayController.OriginatorGameObject != originatorGameObject)
            {
                Debug.LogError("The passed replayable must match the current recording replayable!");
                return;
            }

            if (!CurrentReplayController.IsRecording)
            {
                Debug.LogWarning("You can't stop recording during a replay!");
                return;
            }
            
            CurrentReplayController.StartReplay(isLoop);
        }

        public void StopReplayable(GameObject originatorGameObject)
        {
            foreach (var replayController in _replayControllerList
                .Where(replayController => replayController.OriginatorGameObject == originatorGameObject))
            {
                replayController.StopReplay();
                return;
            }

            Debug.LogWarning("The passed replayable wasn't found!");
        }

        public void UnregisterReplayable(GameObject originatorGameObject)
        {
            for (var index = _replayControllerList.Count - 1; index >= 0; index--)
            {
                var replayController = _replayControllerList[index];
                if (replayController.OriginatorGameObject != originatorGameObject) continue;
                
                _replayControllerList.RemoveAt(index);
                return;
            }
            
            Debug.LogWarning("The ReplayManager doesn't contain the Replayable and thus weren't removed!");
        }

        public bool IsRecording()
        {
            return _replayControllerList.Any(replayController => replayController.IsRecording);
        }
    }
}