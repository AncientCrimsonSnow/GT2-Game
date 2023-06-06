using UnityEngine;

namespace Features.TileSystem.Item
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
    }
}