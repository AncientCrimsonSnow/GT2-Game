using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayManager : MonoBehaviour
    {
        [SerializeField] private float tickDurationInSeconds;

        public float TickTimeInSeconds => tickDurationInSeconds;
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
        
        public void InitializeRecording(IReplayable replayable)
        {
            if (_replayControllerList.Any(controller => controller.IsRecording))
            {
                Debug.LogError("You can only initialize a new record, when there is no other object currently recording!");
                return;
            }
            
            var replayController = new ReplayController(this, replayable);
            _replayControllerList.Add(replayController);
        }

        public void StartReplay(IReplayable replayable, bool isLoop)
        {
            if (!HasCurrentReplayController)
            {
                Debug.LogError("There is currently no replayable registered!");
                return;
            }

            if (CurrentReplayController.Replayable != replayable)
            {
                Debug.LogError("The passed replayable must match the current recording replayable!");
                return;
            }
            
            CurrentReplayController.StopRecording(isLoop);
        }

        public void UnregisterReplayable(IReplayable replayable)
        {
            for (var index = _replayControllerList.Count - 1; index >= 0; index--)
            {
                var replayController = _replayControllerList[index];
                if (replayController.Replayable != replayable) continue;
                
                _replayControllerList.RemoveAt(index);
            }
        }
    }
}