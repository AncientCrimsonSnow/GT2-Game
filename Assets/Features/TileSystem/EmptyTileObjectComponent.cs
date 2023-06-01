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
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) 
                && !heldItemBehaviour.IsCarrying()) return false;

            var heldItem = heldItemBehaviour.HeldItem;
            var instantiatedObject = TileHelper.InstantiateOnTile(_tileBase, heldItem.prefab, Quaternion.identity);
            var tileObjectComponent = new UnstackableTileObjectComponent(heldItem, instantiatedObject, _tileBase);
            _tileBase.RegisterNewTileObjectComponent(tileObjectComponent);
            return true;
        }

        public void OnUnregister() { }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}