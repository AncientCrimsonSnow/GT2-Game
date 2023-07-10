using UnityEngine;

namespace Uilities.Pool
{
    [DefaultExecutionOrder(-9990), DisallowMultipleComponent]
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] private PoolContainer[] pools;
        
        private void Awake()
        {
            foreach (var poolContainer in pools)
            {
                poolContainer.associatedPrefab.Populate(poolContainer.additionalReusableCount);
            }
        }
        
 #region Testing
        //[SerializeField] private Poolable pooled;
        
        private void Testing(PoolContainer poolContainer)
        {
            //TODO: When the Poolable Component isn't attached to a Prefab, the Poolable reference cant be collected
            //var poolable = poolContainer.associatedPrefab.GetComponent<Poolable>();
            //poolable.Populate(poolContainer.additionalReusableCount);       
                
            poolContainer.associatedPrefab.Populate(poolContainer.additionalReusableCount);
            poolContainer.associatedPrefab.TryClear(ClearTag.All);
            //poolable.Populate(poolContainer.additionalReusableCount);
            poolContainer.associatedPrefab.TryDestroyPool();
            poolContainer.associatedPrefab.Populate(poolContainer.additionalReusableCount);
            //poolable.TryDestroyPool();
            //poolable.Populate(poolContainer.additionalReusableCount);
            //poolable.TryClear(ClearTag.All);
                
            //var pooled5 = pooled.AssociatedPrefab.Reuse();      
            //var pooled1 = pooled.Reuse();
                
            var pooled2 = poolContainer.associatedPrefab.Reuse();
            //poolContainer.associatedPrefab.Populate(poolContainer.additionalReusableCount);
            //var pooled3 = poolable.Reuse();
            var pooled4 = poolContainer.associatedPrefab.Reuse();
                
            //pooled.Release();
            pooled4.Release();
            pooled4.Release();
                
            pooled4.SetPoolingEnabled(false);
            pooled4.Release();
            pooled4.SetPoolingEnabled(true);
            pooled4.Release();
        }
#endregion

    }
}
