using System;
using UnityEngine;

namespace Uilities.Pool
{
    //TODO: enum to check, if list addition to pool is valid
    
    public enum PoolingType { None, CanCreatePool, CanCreatePopulatedPool }
    
    [DefaultExecutionOrder(-9999), DisallowMultipleComponent]
    public sealed class Poolable : MonoBehaviour
    {
        [SerializeField] private PoolContainer poolContainer;
        [SerializeField] private PoolingType poolingType;

        public bool IsPoolingEnabled { get; private set; }

        public GameObject AssociatedPrefab
        {
            get
            {
                if (_associatedPrefab) return _associatedPrefab;

                if (poolContainer && poolContainer.associatedPrefab) return poolContainer.associatedPrefab;

                return null;
            }
        }

        private IBeforeReusePoolableCallback[] _beforeReusePoolableCallbackChildren = Array.Empty<IBeforeReusePoolableCallback>();
        private IBeforeReleasePoolableCallback[] _beforeReleasePoolableCallbackChildren = Array.Empty<IBeforeReleasePoolableCallback>();

        private GameObject _associatedPrefab;
        private int _additionalReusableCount;
        private Pool _pool;
        private bool _isInitialized;
        private bool _hasCreatedPool;

        private void Awake()
        {
            IsPoolingEnabled = true;
            
            _beforeReusePoolableCallbackChildren = GetComponentsInChildren<IBeforeReusePoolableCallback>(true);
            _beforeReleasePoolableCallbackChildren = GetComponentsInChildren<IBeforeReleasePoolableCallback>(true);
            
            if (!poolContainer || !poolContainer.associatedPrefab || poolingType == PoolingType.None) return;

            InitializePoolable(poolContainer.associatedPrefab, poolContainer.additionalReusableCount);
        }

        private void Start()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"Initialization of the Pool failed! You need to assign a 'PoolContainer' to the '{this}' (Scene Object)!" +
                                 $"Otherwise you might have already destroyed this GameObject, but are still referencing to this component!");
                return;
            }

            if (_hasCreatedPool && poolingType == PoolingType.CanCreatePopulatedPool)
            {
                _pool.Populate(_additionalReusableCount);
            }
        }
        
        private void OnEnable()
        {
            if (!_isInitialized) return;

            SetReleasable();
        }
        
        private void OnDisable()
        {
            if (!_isInitialized) return;
            
            SetReusable();
        }
        
        private void OnDestroy()
        {
            if (!_isInitialized) return;

            _isInitialized = false;
            _pool.UnregisterToReleasable(this).UnregisterToReusable(this).UnregisterToInactive(this);
        }
        
        public void InitializePoolable(GameObject newAssociatedPrefab, int additionalReusableCount)
        {
            if (_isInitialized) return;

            if (!poolContainer)
            {
                poolContainer = ScriptableObject.CreateInstance(typeof(PoolContainer)) as PoolContainer;
                if (poolContainer != null)
                {
                    poolContainer.associatedPrefab = newAssociatedPrefab;
                    poolContainer.additionalReusableCount = additionalReusableCount;
                }
            }

            _associatedPrefab = newAssociatedPrefab;
            _additionalReusableCount = additionalReusableCount;

            if (!PoolManager.TryGetPool(this, out _pool))
            {
                _hasCreatedPool = true;
                _pool = PoolManager.CreatePool(this);
            }
            
            _isInitialized = true;
        }
        
        public void SetPoolingEnabled(bool isPoolingEnabled)
        {
            if (!_isInitialized) return;
            
            if (isPoolingEnabled)
            {
                EnablePooling();
            }
            else
            {
                DisablePooling();
            }
        }

        private void EnablePooling()
        {
            IsPoolingEnabled = true;
            if (gameObject.activeInHierarchy)
            {
                SetReleasable();
            }
            else
            {
                SetReusable();
            }

            _pool.UnregisterToInactive(this);
        }

        private void DisablePooling()
        {
            _pool.UnregisterToReleasable(this).UnregisterToReusable(this).RegisterToInactive(this);
            IsPoolingEnabled = false;
        }
        
        public void OnBeforeReusePoolable()
        {
            if (!_isInitialized) return;
			
            foreach (var childPoolable in _beforeReusePoolableCallbackChildren)
            {
                childPoolable.OnBeforeReusePoolable();
            }
        }

        public void OnBeforeReleasePoolable()
        {
            if (!_isInitialized) return;
            
            foreach (var childPoolable in _beforeReleasePoolableCallbackChildren)
            {
                childPoolable.OnBeforeReleasePoolable();
            }
        }

        private void SetReleasable()
        {
            if (!IsPoolingEnabled) return;
            
            _pool.RegisterToReleasable(this).UnregisterToReusable(this);
        }
        
        private void SetReusable()
        {
            if (!IsPoolingEnabled) return;
            
            _pool.RegisterToReusable(this).UnregisterToReleasable(this);
        }
    }
}
