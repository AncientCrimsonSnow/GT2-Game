using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Uilities.Pool
{
    public enum ClearTag { Reusable, Releasable, Inactive, All }
    
    [Serializable]
    public class Pool
    {
        private GameObject _prefabIdentifier;
        private Poolable _poolableIdentifier;

        private List<Poolable> _reusableObjects;
        private List<Poolable> _releasableObjects;
        private List<Poolable> _inactiveObjects;

        public Pool(Poolable poolable, Action<Pool, GameObject> onPoolCreated)
        {
            _prefabIdentifier = poolable.AssociatedPrefab;
            onPoolCreated.Invoke(this, _prefabIdentifier);
            
            _reusableObjects = new List<Poolable>();
            _releasableObjects = new List<Poolable>();
            _inactiveObjects = new List<Poolable>();
            
            _poolableIdentifier = Object.Instantiate(poolable);
            Object.DontDestroyOnLoad(_poolableIdentifier);
            _poolableIdentifier.OnBeforeReleasePoolable();
            _poolableIdentifier.gameObject.SetActive(false);
        }

        public Pool(GameObject identifierPrefab, Action<Pool, GameObject> onPoolCreated)
        {
            _prefabIdentifier = identifierPrefab;
            onPoolCreated.Invoke(this, _prefabIdentifier);
            
            _reusableObjects = new List<Poolable>();
            _releasableObjects = new List<Poolable>();
            _inactiveObjects = new List<Poolable>();
            
            if (!identifierPrefab.TryGetComponent(out Poolable poolable))
            {
                _poolableIdentifier = Object.Instantiate(identifierPrefab).AddComponent<Poolable>();
                _poolableIdentifier.InitializePoolable(identifierPrefab, 1);
            }
            else
            {
                _poolableIdentifier = Object.Instantiate(poolable);
                if (!poolable.AssociatedPrefab)
                {
                    _poolableIdentifier.InitializePoolable(identifierPrefab, 1);
                }
            }
            Object.DontDestroyOnLoad(_poolableIdentifier);
            _poolableIdentifier.OnBeforeReleasePoolable();
            _poolableIdentifier.gameObject.SetActive(false);
        }

        public bool IsPoolableIdentifier(Poolable checkedPoolable)
        {
            return checkedPoolable == _poolableIdentifier;
        }

        public Pool Populate(int additionalReusableCount)
        {
            CreateInstances(additionalReusableCount, InstantiateAction, poolable => poolable.gameObject.SetActive(false));
            return this;
        }
        
        public Pool Populate(int additionalReusableCount, Vector3 position, Quaternion rotation)
        {
            CreateInstances(additionalReusableCount, () => InstantiateAction(position, rotation), poolable => poolable.gameObject.SetActive(false));
            return this;
        }
        
        public Pool Populate(int additionalReusableCount, Vector3 position, Quaternion rotation, Transform parent)
        {
            CreateInstances(additionalReusableCount, () => InstantiateAction(position, rotation, parent), poolable => poolable.gameObject.SetActive(false));
            return this;
        }
        
        public Pool Populate(int additionalReusableCount, Transform parent)
        {
            CreateInstances(additionalReusableCount, () => InstantiateAction(parent), poolable => poolable.gameObject.SetActive(false));
            return this;
        }
        
        public Pool Populate(int additionalReusableCount, Transform parent, bool worldPositionStays)
        {
            CreateInstances(additionalReusableCount, () => InstantiateAction(parent, worldPositionStays), poolable => poolable.gameObject.SetActive(false));
            return this;
        }

        public Poolable Reuse()
        {
            return GetInstance(InstantiateAction);
        }
        
        public Poolable Reuse(Vector3 position, Quaternion rotation)
        {
            return GetInstance(() => InstantiateAction(position, rotation), poolable => ReuseAction(poolable, position, rotation));
        }
        
        public Poolable Reuse(Vector3 position, Quaternion rotation, Transform parent)
        {
            return GetInstance(() => InstantiateAction(position, rotation, parent), poolable => ReuseAction(poolable, position, rotation, parent));
        }
        
        public Poolable Reuse(Transform parent)
        {
            return GetInstance(() => InstantiateAction(parent), poolable => ReuseAction(poolable, parent));
        }
        
        public Poolable Reuse(Transform parent, bool worldPositionStays)
        {
            return GetInstance(() => InstantiateAction(parent), poolable => ReuseAction(poolable, parent, worldPositionStays));
        }

        public void Release(Poolable instance)
        {
            instance.OnBeforeReleasePoolable();
            instance.gameObject.SetActive(false);
        }
        
        public Pool Clear(ClearTag clearTag, params ClearTag[] additionalClearTags)
        {
            ClearByTag(clearTag);
            
            foreach (var additionalClearTag in additionalClearTags)
            {
                ClearByTag(additionalClearTag);
            }
            
            return this;
        }

        public void DestroyPool()
        {
            Clear(ClearTag.All);
            Object.Destroy(_poolableIdentifier.gameObject);
            
            _prefabIdentifier = null;
            _poolableIdentifier = null;

            _reusableObjects = null;
            _releasableObjects = null;
            _inactiveObjects = null;
        }

        public Pool RegisterToInactive(Poolable poolable)
        {
            if (IsValidInactiveListManagement(poolable, _inactiveObjects))
            {
                _inactiveObjects.Add(poolable);
            }
            return this;
        }

        public Pool UnregisterToInactive(Poolable poolable)
        {
            if (IsValidActiveListManagement(poolable, _inactiveObjects))
            {
                _inactiveObjects.Remove(poolable);
            }
            return this;
        }

        public Pool RegisterToReleasable(Poolable poolable)
        {
            if (IsValidActiveListManagement(poolable, _releasableObjects))
            {
                _releasableObjects.Add(poolable);
            }

            return this;
        }
        
        public Pool UnregisterToReleasable(Poolable poolable)
        {
            if (IsValidActiveListManagement(poolable, _releasableObjects))
            {
                _releasableObjects.Remove(poolable);
            }
            return this;
        }
        
        public Pool RegisterToReusable(Poolable poolable)
        {
            if (IsValidActiveListManagement(poolable, _reusableObjects))
            {
                _reusableObjects.Add(poolable);
            }
            return this;
        }
        
        public Pool UnregisterToReusable(Poolable poolable)
        {
            if (IsValidActiveListManagement(poolable, _reusableObjects))
            {
                _reusableObjects.Remove(poolable);
            }
            return this;
        }
        
        private Poolable InstantiateAction()
        {
            return Object.Instantiate(_poolableIdentifier);
        }
        
        private Poolable InstantiateAction(Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(_poolableIdentifier, position, rotation);
        }
        
        private Poolable InstantiateAction(Vector3 position, Quaternion rotation, Transform parent)
        {
            return Object.Instantiate(_poolableIdentifier, position, rotation, parent);
        }
        
        private Poolable InstantiateAction(Transform parent)
        {
            return Object.Instantiate(_poolableIdentifier, parent);
        }
        
        private Poolable InstantiateAction(Transform parent, bool worldPositionStays)
        {
            return Object.Instantiate(_poolableIdentifier, parent, worldPositionStays);
        }
        
        private void ReuseAction(Poolable poolable, Vector3 position, Quaternion rotation)
        {
            poolable.transform.SetPositionAndRotation(position, rotation);
        }
        
        private void ReuseAction(Poolable poolable, Vector3 position, Quaternion rotation, Transform parent)
        {
            poolable.transform.SetPositionAndRotation(position, rotation);
            poolable.transform.SetParent(parent);
        }
        
        private void ReuseAction(Poolable poolable, Transform parent)
        {
            poolable.transform.SetParent(parent);
        }
        
        private void ReuseAction(Poolable poolable, Transform parent, bool worldPositionStays)
        {
            poolable.transform.SetParent(parent, worldPositionStays);
        }

        private bool IsValidInactiveListManagement(Poolable poolable, List<Poolable> poolables) =>
            poolable.IsPoolingEnabled && _poolableIdentifier != null && poolable != _poolableIdentifier &&
            poolables != null;
        
        private bool IsValidActiveListManagement(Poolable poolable, List<Poolable> poolables) =>
            poolable.IsPoolingEnabled && _poolableIdentifier != null && poolable != _poolableIdentifier &&
            poolables != null;
        
        private void ClearByTag(ClearTag clearTag)
        {
            switch (clearTag)
            {
                case ClearTag.All:
                    Clear(_reusableObjects);
                    Clear(_releasableObjects);
                    Clear(_inactiveObjects);
                    break;
                case ClearTag.Reusable:
                    Clear(_reusableObjects);
                    break;
                case ClearTag.Releasable:
                    Clear(_releasableObjects);
                    break;
                case ClearTag.Inactive:
                    Clear(_inactiveObjects);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private Poolable GetInstance(Func<Poolable> instantiateMethod, Action<Poolable> enableMethod = null)
        {
            var count = _reusableObjects.Count;
            if (count == 0)
            {
                return CreateInstance(instantiateMethod);
            }
            
            var instance = _reusableObjects[^1];
            enableMethod?.Invoke(instance);
            instance.OnBeforeReusePoolable();
            instance.gameObject.SetActive(true);

            return instance;
        }

        private Poolable[] CreateInstances(int additionalReusableCount, Func<Poolable> instantiateMethod, Action<Poolable> onCreated = null)
        {
            if (additionalReusableCount < 0)
            {
                Debug.LogWarning("Invalid count to create instances of a poolable! It got set to 0 by default!");
                additionalReusableCount = 0;
            }

            var poolables = new Poolable[additionalReusableCount];
            
            //Instantiating a runtime GameObject will copy the 'ActiveSelf' of that GameObject.
            //An instantiated GameObject that is inactive, wont call Unity event functions like 'Awake', 'Start', 'OnEnable' ...
            //To prevent that, the '_poolableIdentifier' is set active before instantiating the clones.
            _poolableIdentifier.gameObject.SetActive(true);
            
            for (var i = 0; i < additionalReusableCount; i++)
            {
                poolables[i] = instantiateMethod.Invoke();
                onCreated?.Invoke(poolables[i]);
            }
            
            _poolableIdentifier.gameObject.SetActive(false);

            return poolables;
        }
        
        private Poolable CreateInstance(Func<Poolable> instantiateMethod, Action<Poolable> onCreated = null)
        {
            return CreateInstances(1, instantiateMethod, onCreated)[0];
        }
        
        private void Clear(List<Poolable> poolables)
        {
            for (var index = poolables.Count - 1; index >= 0; index--)
            {
                var poolable = poolables[index];
                
                Object.Destroy(poolable.gameObject);
            }
            
            poolables.Clear();
        }
    }
}
