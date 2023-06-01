using Features.TileSystem;
using UnityEngine;

namespace DataStructures.StateLogic
{
    //TODO: what is the point of this thing? Add this, when it has an item. If it doesnt, put he empty inside.
    public class TileObjectDecorator : ITileObjectComponent
    {
        public BaseTileObjectFactory NewItem { get; }
        public GameObject InstantiatedObject { get; }
        public TileBase TileBase { get; }

        public ITileObjectComponent TileObjectComponent { get; set; }

        public TileObjectDecorator(BaseTileObjectFactory newItem, GameObject instantiatedObject, TileBase tileBase)
        {
            NewItem = newItem;
            InstantiatedObject = instantiatedObject;
            TileBase = tileBase;
        }
        
        public bool OnActiveInteract(GameObject interactor)
        {
            return TileObjectComponent.OnActiveInteract(interactor);
        }

        public bool IsMovable()
        {
            return TileObjectComponent.IsMovable();
        }

        public bool CanContainObject()
        {
            return TileObjectComponent.CanContainObject();
        }
    }
}