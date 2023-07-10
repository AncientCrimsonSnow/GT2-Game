using UnityEngine;
using Object = UnityEngine.Object;

namespace Uilities.Pool
{
	public static class PoolHelper
	{
		public static void Populate(this GameObject prefab, int additionalReusableCount)
		{
			prefab.GetPool().Populate(additionalReusableCount);
		}
		
		public static void Populate(this GameObject prefab, int additionalReusableCount, Vector3 position, Quaternion rotation)
		{
			prefab.GetPool().Populate(additionalReusableCount, position, rotation);
		}
		
		public static void Populate(this GameObject prefab, int additionalReusableCount, Vector3 position, Quaternion rotation, Transform parent)
		{
			prefab.GetPool().Populate(additionalReusableCount, position, rotation, parent);
		}
		
		public static void Populate(this GameObject prefab, int additionalReusableCount, Transform parent)
		{
			prefab.GetPool().Populate(additionalReusableCount, parent);
		}
		
		public static void Populate(this GameObject prefab, int additionalReusableCount, Transform parent, bool worldPositionStays)
		{
			prefab.GetPool().Populate(additionalReusableCount, parent, worldPositionStays);
		}

		public static void Populate(this Poolable poolable, int additionalReusableCount)
		{
			poolable.GetPool().Populate(additionalReusableCount);
		}
		
		public static void Populate(this Poolable poolable, int additionalReusableCount, Vector3 position, Quaternion rotation)
		{
			poolable.GetPool().Populate(additionalReusableCount, position, rotation);
		}
		
		public static void Populate(this Poolable poolable, int additionalReusableCount, Vector3 position, Quaternion rotation, Transform parent)
		{
			poolable.GetPool().Populate(additionalReusableCount, position, rotation, parent);
		}
		
		public static void Populate(this Poolable poolable, int additionalReusableCount, Transform parent)
		{
			poolable.GetPool().Populate(additionalReusableCount, parent);
		}
		
		public static void Populate(this Poolable poolable, int additionalReusableCount, Transform parent, bool worldPositionStays)
		{
			poolable.GetPool().Populate(additionalReusableCount, parent, worldPositionStays);
		}
		
		public static bool TryDestroyPool(this GameObject prefab)
		{
			var res = PoolManager.TryGetPool(prefab, out Pool pool);

			if (res)
			{
				PoolManager.DestroyPool(pool);
			}

			return res;
		}
		
		public static bool TryDestroyPool(this Poolable poolable)
		{
			var res = PoolManager.TryGetPool(poolable, out Pool pool);

			if (res)
			{
				PoolManager.DestroyPool(pool);
			}

			return res;
		}

		public static bool TryClear(this GameObject prefab, ClearTag clearTag, params ClearTag[] additionalClearTags)
		{
			if (!PoolManager.TryGetPool(prefab, out Pool pool)) return false;
            
			pool.Clear(clearTag, additionalClearTags);
			
			return true;
		}
		
		public static bool TryClear(this Poolable prefab, ClearTag clearTag, params ClearTag[] additionalClearTags)
		{
			if (!PoolManager.TryGetPool(prefab, out Pool pool)) return false;
            
			pool.Clear(clearTag, additionalClearTags);
			
			return true;
		}

		public static Poolable Reuse(this GameObject prefab)
		{
			return prefab.GetPool().Reuse();
		}
		
		public static Poolable Reuse(this GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return prefab.GetPool().Reuse(position, rotation);
		}
		
		public static Poolable Reuse(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			return prefab.GetPool().Reuse(position, rotation, parent);
		}
		
		public static Poolable Reuse(this GameObject prefab, Transform parent)
		{
			return prefab.GetPool().Reuse(parent);
		}
		
		public static Poolable Reuse(this GameObject prefab, Transform parent, bool worldPositionStays)
		{
			return prefab.GetPool().Reuse(parent, worldPositionStays);
		}
		

		public static Poolable Reuse(this Poolable poolable)
		{
			return poolable.GetPool().Reuse();
		}

		public static Poolable Reuse(this Poolable poolable, Vector3 position, Quaternion rotation)
		{
			return poolable.GetPool().Reuse(position, rotation);
		}
		
		public static Poolable Reuse(this Poolable poolable, Vector3 position, Quaternion rotation, Transform parent)
		{
			return poolable.GetPool().Reuse(position, rotation, parent);
		}
		
		public static Poolable Reuse(this Poolable poolable, Transform parent)
		{
			return poolable.GetPool().Reuse(parent);
		}
		
		public static Poolable Reuse(this Poolable poolable, Transform parent, bool worldPositionStays)
		{
			return poolable.GetPool().Reuse(parent, worldPositionStays);
		}
		
		public static void Release(this Poolable instance, bool destroyIfInactive = false)
		{
			var isPooled = PoolManager.TryGetPool(instance, out Pool pool);

			if (!isPooled)
			{
				Object.Destroy(instance.gameObject);
				return;
			}
			
			if (instance.IsPoolingEnabled)
			{
				pool.Release(instance);
			}
			else
			{
				if (destroyIfInactive)
				{
					Object.Destroy(instance.gameObject);
				}
			}
		}
	}
}
