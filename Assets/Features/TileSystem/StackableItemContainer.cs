using UnityEngine;

namespace Features.TileSystem
{
    public class StackableItemContainer : ItemContainerBase
    {
        private readonly int _maxStack;
        private int _itemCount;

        public StackableItemContainer(string itemName, GameObject instantiatedObject, int maxStack) 
            : base(itemName, instantiatedObject)
        {
            _maxStack = maxStack;
        }
        
        public override bool OnActiveInteract(GameObject interactor)
        {
            //TODO: placing items on top of stack, must be done inside this script? For MaxStack necessary!
            
            if (_itemCount == 0) return false;
            
            _itemCount--;
            Debug.Log($"Picking up {ItemName}");
            
            return true;
        }
    }
}