using UnityEngine;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class BaseItem_SO : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
        public int itemValue;

        public virtual bool CanCast(GameObject caster, out string interactionText)
        {
            interactionText = "";
            return false;
        }
        
        public virtual bool TryCast(GameObject caster)
        {
            return false;
        }
    }
}