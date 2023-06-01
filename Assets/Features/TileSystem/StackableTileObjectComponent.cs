using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public class StackableTileObjectComponent : ITileObjectComponent
    {
        public GameObject InstantiatedObject { get; }
        public TileBase TileBase { get; }
        
        private readonly BaseItem _newItem;
        private int _itemCount;

        public StackableTileObjectComponent(BaseItem newItem, GameObject instantiatedObject, TileBase tileBase)
        {
            _newItem = newItem;
            InstantiatedObject = instantiatedObject;
            TileBase = tileBase;
        }
        
        public bool OnActiveInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour)) return false;
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (_itemCount >= _newItem.maxStack)
                {
                    Debug.LogWarning($"Cant add {heldItemBehaviour.HeldItem}! {GetType()} has reached it's max item count!");
                    return false;
                }
                
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (_itemCount <= 0) return false;
                
                _itemCount--;
                heldItemBehaviour.PickupItem(_newItem);
            }
            
            return true;
        }

        public void OnUnregister()
        {
            Object.Destroy(InstantiatedObject);
        }

        public bool IsMovable() => true;

        public bool CanContainObject() => true;
    }
}