using Uilities.Pool;
using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    /// <summary>
    /// This script automatically registers itself to the TileManager dependant on the position at Start.
    /// When changing it's position later, it will still be inside the same position inside the TileManager!
    /// Tiles must currently be static on a position!
    /// </summary>
    public abstract class BaseTileRegistrator : MonoBehaviour, IBeforeReusePoolableCallback, IBeforeReleasePoolableCallback
    {
        [SerializeField] private TileManager tileManager;

        public Tile Tile => tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        
        private Vector3 _registeredPosition;
        public bool IsCurrentlyRegistered { get; private set; }

        public void Awake()
        {
            if (!CanRegisterOnTile()) return;
            
            ApplyRoundedPosition();
            RegisterOnTile();
        }

        public void OnBeforeReusePoolable()
        {
            if (!CanRegisterOnTile()) return;
            
            ApplyRoundedPosition();
            RegisterOnTile();
        }

        public void OnBeforeReleasePoolable()
        {
            UnregisterOnTile();
        }
        
        public void OnDestroy()
        {
            UnregisterOnTile();
        }
        
        public virtual bool CanRegisterOnTile()
        {
            return !IsCurrentlyRegistered;
        }

        private void RegisterOnTile()
        {
            IsCurrentlyRegistered = true;
            _registeredPosition = transform.position;
            InternalRegisterOnTile();
        }
        
        protected abstract void InternalRegisterOnTile();

        private void UnregisterOnTile()
        {
            if (!IsCurrentlyRegistered) return;
            IsCurrentlyRegistered = false;
            
            if (!_registeredPosition.Equals(transform.position))
            {
                Debug.LogWarning($"You changed the position of {this} during Runtime! It will still unregister itself from the registered Tile!");
            }
            
            InternalUnregisterOnTile();
        }

        protected abstract void InternalUnregisterOnTile();

        private void ApplyRoundedPosition()
        {
            if (_registeredPosition.x % 1 != 0 || _registeredPosition.z % 1 != 0)
            {
                Debug.LogWarning("Position of this Tile got mathematically rounded, because it isn't on a whole number!");
            
                _registeredPosition = new Vector3(Mathf.RoundToInt(_registeredPosition.x), _registeredPosition.y, Mathf.RoundToInt(_registeredPosition.z));
                transform.position = _registeredPosition;
            }
        }
    }
}
