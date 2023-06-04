using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
        public int maxStack;
    }
}