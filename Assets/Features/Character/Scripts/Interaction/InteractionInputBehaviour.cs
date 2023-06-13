using System;
using Features.ReplaySystem;
using Features.ReplaySystem.Record;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Interaction
{
    public class InteractionInputBehaviour : BaseInteractionInput, IReplayOriginator
    {
        public Action<IInputSnapshot> PushNewTick { get; set; }
    
        [SerializeField] private TileManager tileManager;

        private void Start()
        {
            ReplayManager.Instance.RegisterOriginator(gameObject, this);
        }

        public override void OnInteractionInput(InputAction.CallbackContext context)
        {
            if (ReplayManager.Instance.IsTickPerformed) return;
        
            PushNewTick.Invoke(new InteractionInputSnapshot(gameObject, tileManager));
        }
    }
}
