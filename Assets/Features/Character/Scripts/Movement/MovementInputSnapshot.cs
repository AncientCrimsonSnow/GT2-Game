using DG.Tweening;
using Features.ReplaySystem;
using Features.ReplaySystem.Record;
using Features.TileSystem.Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Character.Scripts.Movement
{
    public class MovementInputSnapshot : IInputSnapshot
    {
        private readonly Transform _transform;
        private readonly TileManager _tileManager;
        private readonly Animator _animator;
        private readonly Vector2 _storedInputVector;
        private readonly Ease _easeType;
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        public MovementInputSnapshot(Transform transform, Animator animator, TileManager tileManager, Vector2 storedInputVector, Ease easeType)
        {
            _transform = transform;
            _animator = animator;
            _tileManager = tileManager;
            _storedInputVector = storedInputVector;
            _easeType = easeType;
        }
    
        public void Tick(float tickDurationInSeconds)
        {
            var inputInt2 = new int2(Mathf.RoundToInt(_storedInputVector.x), Mathf.RoundToInt(_storedInputVector.y));
            var targetTile = _tileManager.GetTileAt(TileHelper.TransformPositionToInt2(_transform) + inputInt2);
            
            if (targetTile.IsMovable())
            {
                TweenMove(tickDurationInSeconds);
                
                _transform.rotation =
                    Quaternion.LookRotation(new Vector3(_storedInputVector.x, _transform.position.y, _storedInputVector.y));
            }
            else
            {
                ReplayManager.Instance.StopReplayable(_transform.gameObject);
            }
        }
    
        private void TweenMove(float tickDurationInSeconds)
        {
            var inputMovement = new Vector3(_storedInputVector.x, 0, _storedInputVector.y);
            var position = TileHelper.TransformPositionToVector3Int(_transform);
            _transform.DOMove(position + inputMovement, tickDurationInSeconds)
                .SetEase(_easeType)
                .OnStart(() => _animator.SetBool(IsMoving, true))
                .OnComplete(() => _animator.SetBool(IsMoving, false));
        }
    }
}
