using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    /// <summary>
    /// This script automatically registers itself to the TileManager dependant on the position at Start.
    /// When changing it's position later, it will still be inside the same position inside the TileManager!
    /// Tiles must currently be static on a position!
    ///
    /// Current suitable concept for pooling: setting out-of-screenspace objects inactive. It got registered inside the TileManager.
    /// Thus, things will be able to interact with it, even though it is set inactive & out of screenspace (useful for the tick system).
    /// </summary>
    public abstract class BaseTileRegistrator : MonoBehaviour
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private bool registerOnStart;

        public Tile Tile => tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
    
        private Vector3 _registeredPosition;

        private void Start()
        {
            ApplyRoundedPosition();

            if (!registerOnStart) return;

            if (CanRegisterOnTile())
            {
                RegisterOnTile();
            }
        }
    
        private void ApplyRoundedPosition()
        {
            if (_registeredPosition.x % 1 != 0 || _registeredPosition.z % 1 != 0)
            {
                Debug.LogWarning("Position of this Tile got mathematically rounded, because it isn't on a whole number!");
            
                _registeredPosition = new Vector3(Mathf.RoundToInt(_registeredPosition.x), _registeredPosition.y, Mathf.RoundToInt(_registeredPosition.z));
                transform.position = _registeredPosition;
            }
        }

        /// <summary>
        /// Method for defining, if this interactable can be registered -> also relevant for buildingKernel
        /// </summary>
        /// <returns></returns>
        public virtual bool CanRegisterOnTile() => true;

        public void RegisterOnTile()
        {
            _registeredPosition = transform.position;
            InternalRegisterOnTile();
        }

        protected abstract void InternalRegisterOnTile();

        protected virtual void OnDestroy()
        {
            var position = transform.position;
            
            if (!CanUnregisterOnTile()) return;
            
            if (!_registeredPosition.Equals(position))
            {
                Debug.LogWarning($"You changed the position of {this} during Runtime! It will still unregister itself from the registered Tile!");
            }
            
            UnregisterOnTile();
        }

        //TODO: this needs to be a "can destroy tile interactable"
        protected virtual bool CanUnregisterOnTile() => true;

        protected abstract void UnregisterOnTile();
    }
}
