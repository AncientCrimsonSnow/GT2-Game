using UnityEngine;

namespace Features.TileSystem
{
    public class CarriedItemBehaviour : MonoBehaviour
    {
        public BaseTileObjectFactory HeldItem { get; private set; }
        
        public bool IsCarrying() => HeldItem != null;
        
        public void DropItem(TileBase currentTile)
        {
            RemoveItem();
            
            HeldItem.InstantiateAndRegister(currentTile, Quaternion.identity);
        }

        public void RemoveItem()
        {
            if (HeldItem == null)
            {
                Debug.LogWarning("You are currently not carrying any item");
            }
            
            Debug.Log($"Removing up {HeldItem.name}");
            HeldItem = null;
        }

        public void PickupItem(BaseTileObjectFactory newItem)
        {
            if (HeldItem != null)
            {
                Debug.LogWarning($"{name} is already Carrying the item {HeldItem.name}! You cant pick up {newItem}!");
            }

            Debug.Log($"Picking up {newItem.name}");
            HeldItem = newItem;
        }
    }
}