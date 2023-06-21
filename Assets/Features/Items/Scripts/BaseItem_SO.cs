using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class BaseItem_SO : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
        [FormerlySerializedAs("tryBuildOnNeighbourDrop")] public bool isBuildingOrigin;

        public virtual bool TryCastMagic(GameObject caster)
        {
            return false;
        }
    }
}