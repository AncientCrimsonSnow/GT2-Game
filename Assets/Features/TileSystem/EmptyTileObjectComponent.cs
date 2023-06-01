using System;
using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public class EmptyTileObjectComponent : ITileObjectComponent
    {
        private readonly TileBase _tileBase;
        public EmptyTileObjectComponent(TileBase tileBase)
        {
            _tileBase = tileBase;
        }

        public bool OnActiveInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBehaviour heldItemBehaviour) 
                && !heldItemBehaviour.IsCarrying()) return false;
            
            heldItemBehaviour.DropItem(_tileBase);      //TODO: create unstackable
            
            var tileObjectDecorator = new TileObjectDecorator()
            tileObjectDecorator.TileObjectComponent = new UnstackableTileObjectComponent(tileObjectDecorator);
            _tileBase.SetTileObjectComponent();
            //TODO: create decorator, & add this
            return true;
        }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}