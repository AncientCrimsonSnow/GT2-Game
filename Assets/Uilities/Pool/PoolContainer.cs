using UnityEngine;

namespace Uilities.Pool
{
    [CreateAssetMenu(fileName = "PoolContainer", menuName = "PoolContainer")]
    public class PoolContainer : ScriptableObject
    {
        public GameObject associatedPrefab;
        [Min(1)] public int additionalReusableCount;
    }
}
