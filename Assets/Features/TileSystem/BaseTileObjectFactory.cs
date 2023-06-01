using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class BaseTileObjectFactory : ScriptableObject
    {
        public bool InstantiateAndRegister(TileBase tileBase, Quaternion rotation, Transform parent)
        {
            var item = InternalGenerateAndRegister(tileBase, rotation, parent);
            return tileBase.SetTileObjectComponent(item);
        }
        
        public bool InstantiateAndRegister(TileBase tileBase, Quaternion rotation)
        {
            var item = InternalGenerateAndRegister(tileBase, rotation);
            return tileBase.SetTileObjectComponent(item);
        }
        
        //TODO: cant return ITileObjectComponent here
        protected abstract ITileObjectComponent InternalGenerateAndRegister(TileBase tileBase, Quaternion rotation, Transform parent = null);
    }
}