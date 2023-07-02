using System;
using System.Collections;
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
        [SerializeField] private Animator animator;
        
        private static readonly int IsWorking = Animator.StringToHash("isWorking");

        private void Start()
        {
            ReplayManager.Instance.RegisterOriginator(gameObject, this);
        }

        public override void OnInteractionInput(InputAction.CallbackContext context)
        {
            if (ReplayManager.Instance.IsTickPerformed) return;
        
            PushNewTick.Invoke(new ActionSnapshot(PerformTick));
        }

        private void PerformTick(float tickDurationInSeconds)
        {
            var registeredPosition = TileHelper.TransformPositionToInt2(transform);
            var tile = tileManager.GetTileAt(registeredPosition);
            tile.TryInteract(gameObject);

            StartCoroutine(AnimateWorking(tickDurationInSeconds));
        }
        
        private IEnumerator AnimateWorking(float tickDurationInSeconds)
        {
            animator.SetBool(IsWorking, true);
            yield return new WaitForSeconds(tickDurationInSeconds);
            animator.SetBool(IsWorking, false);
        }
    }
}
