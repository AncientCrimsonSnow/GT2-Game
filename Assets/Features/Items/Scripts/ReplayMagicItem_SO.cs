using System;
using Features.Camera;
using Features.Character.Scripts;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Magic;
using Features.Character.Scripts.Movement;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class ReplayMagicItem_SO : BaseItem_SO
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private GameObject magicInstantiationPrefab;
        
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
        
        [Header("Input Focus")]
        [SerializeField] private DirectionInputFocus directionInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private CastInputFocus castInputFocus;

        private static readonly int IsCasting = Animator.StringToHash("isCasting");

        public override bool CanCast(GameObject caster, out string interactionText)
        {
            interactionText = "";
            if (skeletonFocus.ContainsFocus()) return false;
            var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(caster.transform));
            if (!tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) return false;

            interactionText = "Control";
            
            return true;
        }

        public override bool TryCast(GameObject caster)
        {
            if (skeletonFocus.ContainsFocus()) return false;

            var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(caster.transform));
            if (!tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>())
            {
                return false;
            }
            
            tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(tile));

            var vector3Int = TileHelper.TransformPositionToVector3Int(caster.transform);
            var instantiatedPrefab = Instantiate(magicInstantiationPrefab, vector3Int, Quaternion.identity);
            InitializeFocus(caster, instantiatedPrefab);
            InitializeRecording(caster, instantiatedPrefab);

            return true;
        }
        
        private void InitializeRecording(GameObject caster, GameObject instantiatedPrefab)
        {
            var onDestroyAction = new Action(() =>
            {
                //destroy, if a record gets interrupted
                if (skeletonFocus.ContainsFocus())
                {
                    ResetFocus(caster);
                }

                ReplayManager.Instance.UnregisterReplayable(instantiatedPrefab);
                Destroy(instantiatedPrefab);
            });
            
            var onReplayCompleteAction = new Action(() =>
            {
                onDestroyAction.Invoke();
                TileHelper.DropItemNearestEmptyTile(tileManager, instantiatedPrefab.transform.position, this);
            });
            
            //the newestInstantiatedSkeleton must be buffered, because it will be null during replay
            ReplayManager.Instance.InitializeRecording(skeletonFocus.GetFocus(), () => ResetFocus(caster), onReplayCompleteAction, onDestroyAction);
        }
        
        private void InitializeFocus(GameObject caster, GameObject instantiatedPrefab)
        {
            caster.GetComponentInChildren<Animator>().SetBool(IsCasting, true);
            
            skeletonFocus.SetFocus(instantiatedPrefab);
            cinemachineVirtualCameraFocus.SetFollow(instantiatedPrefab.transform).ApplyFollow();
            directionInputFocus.PushFocus(instantiatedPrefab.GetComponent<BaseMovementInput>());
            castInputFocus.PushFocus(instantiatedPrefab.GetComponent<BaseCastInput>());
            interactionInputFocus.PushFocus(instantiatedPrefab.GetComponent<BaseInteractionInput>());
        }

        private void ResetFocus(GameObject caster)
        {
            caster.GetComponentInChildren<Animator>().SetBool(IsCasting, false);
            
            directionInputFocus.PopFocus();
            interactionInputFocus.PopFocus();
            castInputFocus.PopFocus();
            cinemachineVirtualCameraFocus.RestoreFollow();
            skeletonFocus.Restore();
        }
    }
}
