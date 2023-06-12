using UnityEngine;

namespace Features.TileSystem.ItemSystem
{
    [CreateAssetMenu]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;

        public virtual bool TryCastMagic(GameObject caster)
        {
            return false;
        }
    }
}