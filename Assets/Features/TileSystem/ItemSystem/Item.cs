using UnityEngine;

namespace Features.TileSystem.ItemSystem
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
    }
}