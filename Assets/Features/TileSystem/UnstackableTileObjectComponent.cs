using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableTileObjectComponent : ITileObjectComponent
    {
        private readonly BaseItem _newItem;
        private readonly GameObject _instantiatedObject;
        private readonly TileBase _tileBase;

        public UnstackableTileObjectComponent(BaseItem newItem, GameObject instantiatedObject, TileBase tileBase)
        {
            _newItem = newItem;
            _instantiatedObject = instantiatedObject;
            _tileBase = tileBase;
        }
        
        public bool OnActiveInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) && heldItemBehaviour.IsCarrying()) return false;
            
            heldItemBehaviour.PickupItem(_newItem);
            _tileBase.RegisterNewTileObjectComponent(new EmptyTileObjectComponent(_tileBase));

            return true;
        }

        public void OnUnregister()
        {
            Object.Destroy(_instantiatedObject);
        }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}