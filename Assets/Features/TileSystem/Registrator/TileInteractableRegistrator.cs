using System;
using Features.TileSystem.TileComponents;
using Features.TileSystem.TileSystem;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Features.TileSystem.Registrator
{
    /// <summary>
    /// This script automatically registers itself to the TileManager dependant on the position at Start.
    /// When changing it's position later, it will still be inside the same position inside the TileManager!
    /// Tiles must currently be static on a position!
    ///
    /// Current suitable concept for pooling: setting out-of-screenspace objects inactive. It got registered inside the TileManager.
    /// Thus, things will be able to interact with it, even though it is set inactive & out of screenspace (useful for the tick system).
    /// </summary>
    public abstract class TileInteractableRegistrator : MonoBehaviour
    {
        [SerializeField] private TileManager tileManager;
        
        public Tile Tile { get; private set; }
    
        private ITileInteractable _ownedTileInteractable;
        private int2 _registeredPosition;

        private void Start()
        {
            ApplyRoundedPosition();
            _registeredPosition = TileHelper.TransformPositionToInt2(transform);
            Tile = tileManager.GetTileTypeAt(_registeredPosition);
            
            if (CanRegisterTileInteractable(Tile))
            {
                _ownedTileInteractable = RegisterTileInteractable(Tile);
            }
        }
    
        private void ApplyRoundedPosition()
        {
            var position = transform.position;
            if (position.x % 1 != 0 || position.z % 1 != 0)
            {
                Debug.LogWarning("Position of this Tile got mathematically rounded, because it isn't on a whole number!");
            
                position = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
                transform.position = position;
            }
        }

        protected virtual bool CanRegisterTileInteractable(Tile tile) => true;

        protected abstract ITileInteractable RegisterTileInteractable(Tile tile);

        protected virtual void OnDestroy()
        {
            var position = transform.position;

            if (!_registeredPosition.Equals(new int2((int)position.x, (int)position.z)))
            {
                Debug.LogWarning("You changed the position of this tile during Runtime! It will still unregister itself from the registered Tile!");
            }

            if (_ownedTileInteractable == null) return;

            if (CanUnregisterTileInteractable(_ownedTileInteractable))
            {
                UnregisterTileInteractable(_ownedTileInteractable);
            }
        }

        protected virtual bool CanUnregisterTileInteractable(ITileInteractable tileInteractable) => true;
    
        protected abstract void UnregisterTileInteractable(ITileInteractable tileInteractable);
    }
}
