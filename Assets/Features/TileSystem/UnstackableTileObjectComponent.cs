using DataStructures.StateLogic;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableTileObjectComponent : ITileObjectComponent
    {
        private readonly TileObjectDecorator _tileObjectDecorator;

        public UnstackableTileObjectComponent(TileObjectDecorator tileObjectDecorator)
        {
            _tileObjectDecorator = tileObjectDecorator;
        }
        
        public bool OnActiveInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBehaviour heldItemBehaviour) && heldItemBehaviour.IsCarrying()) return false;
            
            //TODO: destroy instantiated Object
            heldItemBehaviour.PickupItem(_tileObjectDecorator.NewItem);
            _tileObjectDecorator.TileBase.SetTileObjectComponent(new EmptyTileObjectComponent(_tileObjectDecorator.TileBase));

            return true;
        }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}