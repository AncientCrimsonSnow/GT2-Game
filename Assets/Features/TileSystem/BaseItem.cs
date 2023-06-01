using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
        public int maxStack;
    }
}