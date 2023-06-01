using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public class StackableTileObjectComponent : ITileObjectComponent
    {
        private readonly TileObjectDecorator _tileObjectDecorator;
        
        private readonly int _maxItemCount;
        private int _itemCount;

        public StackableTileObjectComponent(TileObjectDecorator tileObjectDecorator , int maxItemCount)
        {
            _tileObjectDecorator = tileObjectDecorator;
            _maxItemCount = maxItemCount;
        }
        
        public bool OnActiveInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBehaviour heldItemBehaviour)) return false;
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (_itemCount >= _maxItemCount)
                {
                    Debug.LogWarning($"Cant add {heldItemBehaviour.HeldItem}! {GetType()} has reached it's max item count!");
                    return false;
                }

                if (_itemCount == 0)
                {
                    //TODO: Instantiate Object
                }
                
                heldItemBehaviour.RemoveItem();
            }
            else
            {
                _itemCount--;
                heldItemBehaviour.PickupItem(_tileObjectDecorator.NewItem);
                
                if (_itemCount == 0)
                {
                    //TODO: destroy instantiated Object
                }
            }
            
            return true;
        }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}