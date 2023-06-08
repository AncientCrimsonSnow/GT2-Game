using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayManager : MonoBehaviour
    {
        [SerializeField] private float tickDurationInSeconds;

        public float TickDurationInSeconds => tickDurationInSeconds;
        public int AdvancedTicks { get; private set; }
        
        private bool HasCurrentReplayController => _replayControllerList.Count > 0;
        private ReplayController CurrentReplayController => _replayControllerList[^1];
        
        private readonly List<ReplayController> _replayControllerList = new List<ReplayController>();
        private bool _tickPerformed;
        private float _tickTimeDelta;
        
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
            if (_tickPerformed)
            {
                Debug.LogWarning("You can't start a new tick, while a tick is being performed!");
                return;
            }
            
            _tickPerformed = true;
            _tickTimeDelta = 0;

            foreach (var replayController in _replayControllerList)
            {
                replayController.Tick();
            }
            AdvancedTicks++;
        }
        
        //TODO: a replayOriginator may have multiple different OriginatorScripts - fix it
        public void InitializeRecording(IReplayOriginator replayOriginator)
        {
            if (_replayControllerList.Any(controller => controller.IsRecording))
            {
                Debug.LogError("You can only initialize a new record, when there is no other object currently recording!");
                return;
            }
            
            if (_replayControllerList.Any(controller => controller.ReplayOriginator == replayOriginator))
            {
                Debug.LogError("A replayable can only be initialized once!");
                return;
            }
            
            var replayController = new ReplayController(this, replayOriginator);
            _replayControllerList.Add(replayController);
        }

        public void StartReplay(IReplayOriginator replayOriginator, bool isLoop, Action onCompleteReplay = null)
        {
            if (!HasCurrentReplayController)
            {
                Debug.LogError("There is currently no replayable registered!");
                return;
            }

            if (CurrentReplayController.ReplayOriginator != replayOriginator)
            {
                Debug.LogError("The passed replayable must match the current recording replayable!");
                return;
            }

            if (!CurrentReplayController.IsRecording)
            {
                Debug.LogWarning("You can't stop recording during a replay!");
                return;
            }
            
            CurrentReplayController.StartReplay(isLoop, () =>
            {
                UnregisterReplayable(replayOriginator);
                onCompleteReplay?.Invoke();
            });
        }

        public void StopReplay(IReplayOriginator replayOriginator)
        {
            foreach (var replayController in _replayControllerList.Where(replayController => replayController.ReplayOriginator == replayOriginator))
            {
                if (replayController.IsRecording)
                {
                    Debug.LogWarning("You can't stop a replay during a record!");
                }
                else
                {
                    replayController.StopReplay();
                }

                return;
            }

            Debug.LogWarning("The passed replayable wasn't found!");
        }

        private void UnregisterReplayable(IReplayOriginator replayOriginator)
        {
            for (var index = _replayControllerList.Count - 1; index >= 0; index--)
            {
                var replayController = _replayControllerList[index];
                if (replayController.ReplayOriginator != replayOriginator) continue;
                
                _replayControllerList.RemoveAt(index);
                return;
            }
            
            Debug.LogWarning("The ReplayManager doesn't contain the Replayable and thus weren't removed!");
        }
    }
}