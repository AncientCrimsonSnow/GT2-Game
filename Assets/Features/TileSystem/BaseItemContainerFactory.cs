using UnityEngine;

namespace Features.TileSystem
{
    public abstract class BaseItemContainerFactory : ScriptableObject
    {
        public abstract IItemContainer GenerateAt(TileBase tileBase, Quaternion rotation, Transform parent = null);
    }
}