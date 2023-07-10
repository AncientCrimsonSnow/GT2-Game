using System;
using System.Collections.Generic;
using UnityEngine;

namespace Uilities.Pool
{
    //TODO: Implement support for GameObjects, that aren't prefabs. (Poolables store their reference prefab)
    //TODO: When a GameObject from a poolable gets destroyed, the poolable shouldn't be usable anymore (something like a disposable - eg UniRX)
    public static class PoolManager
    {
        private static readonly Dictionary<GameObject, Pool> PrefabIdentifierLookup = new Dictionary<GameObject, Pool>();

        public static Pool GetPool(this GameObject prefab)
        {
            if (!TryGetPool(prefab, out Pool pool))
            {
                pool = CreatePool(prefab);
            }
            
            return pool;
        }
        
        public static Pool GetPool(this Poolable prefab)
        {
            if (!TryGetPool(prefab, out Pool pool))
            {
                pool = CreatePool(prefab);
            }

            return pool;
        }

        public static bool TryGetPool(GameObject prefab, out Pool pool)
        {
            return PrefabIdentifierLookup.TryGetValue(prefab, out pool);
        }
        
        public static bool TryGetPool(Poolable poolable, out Pool pool)
        {
            return PrefabIdentifierLookup.TryGetValue(poolable.AssociatedPrefab, out pool);
        }
        
        public static Pool CreatePool(GameObject prefab)
        {
            return new Pool(prefab, RegisterPool);
        }

        public static Pool CreatePool(Poolable poolable)
        {
            return new Pool(poolable, RegisterPool);
        }
        
        public static void DestroyPool(Pool pool)
        {
            UnregisterPool(pool);
            pool.DestroyPool();
        }

        private static void RegisterPool(Pool pool, GameObject prefabIdentifier)
        {
            PrefabIdentifierLookup.Add(prefabIdentifier, pool);
        }
        
        private static void UnregisterPool(Pool pool)
        {
            foreach (var pair in PrefabIdentifierLookup)
            {
                if (pair.Value == pool)
                {
                    PrefabIdentifierLookup.Remove(pair.Key);
                    break;
                }
            }
        }
    }
}
