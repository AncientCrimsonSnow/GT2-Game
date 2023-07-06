using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    /// <summary>
    /// This script automatically registers itself to the TileManager dependant on the position at Start.
    /// When changing it's position later, it will still be inside the same position inside the TileManager!
    /// Tiles must currently be static on a position!
    /// </summary>
    public abstract class BaseTileRegistrator : MonoBehaviour
    {
        [SerializeField] private TileManager tileManager;
        [FormerlySerializedAs("registerOnStart")] [SerializeField] private bool registerOnAwake;

        public Tile Tile => tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        public bool HasRegistratorGroup => TileRegistratorGroup != null;
        private TileRegistratorGroup TileRegistratorGroup { get; set; }
        
        private Action _onDestroyAction;
        private Action _onDisableAction;
        private Vector3 _registeredPosition;
        private bool _isCurrentlyRegistered;
        private bool _hasInitialRegistration;

        private void Awake()
        {
            ApplyRoundedPosition();

            if (!registerOnAwake) return;

            if (CanRegisterOnTile())
            {
                RegisterOnTile();
            }
        }

        private void OnEnable()
        {
            if (!_hasInitialRegistration) return;
            
            Register();
        }

        private void OnDisable()
        {
            Unregister();
            
            _onDisableAction?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            Unregister();

            _onDestroyAction?.Invoke();
        }
        
        public virtual bool CanRegisterOnTile()
        {
            return Tile.IsMovable() && !_isCurrentlyRegistered;
        }
        
        public void RegisterOnTile()
        {
            _isCurrentlyRegistered = true;
            _hasInitialRegistration = true;
            _registeredPosition = transform.position;
            InternalRegisterOnTile();
        }
        
        public void AssignToRegistratorGroup(TileRegistratorGroup tileRegistratorGroup, Action onDisableAction, Action onDestroyAction)
        {
            TileRegistratorGroup = tileRegistratorGroup;
            _onDisableAction = onDisableAction;
            _onDestroyAction = onDestroyAction;
        }

        public void RemoveFromRegistratorGroup()
        {
            TileRegistratorGroup = null;
            _onDisableAction = null;
            _onDestroyAction = null;
        }

        private void Register()
        {
            if (!CanRegisterOnTile()) return;

            RegisterOnTile();
        }

        private void Unregister()
        {
            if (!_isCurrentlyRegistered) return;
            _isCurrentlyRegistered = false;
            
            if (!_registeredPosition.Equals(transform.position))
            {
                Debug.LogWarning($"You changed the position of {this} during Runtime! It will still unregister itself from the registered Tile!");
            }
            
            UnregisterOnTile();
        }

        protected abstract void InternalRegisterOnTile();

        protected abstract void UnregisterOnTile();
        
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
