using DataStructures.StateLogic;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class CarriedItemBaseBehaviour : MonoBehaviour
    {
        public BaseItem HeldItem { get; private set; }
        
        public bool IsCarrying() => HeldItem != null;

        public void DropItem()
        {
            if (HeldItem == null)
            {
                Debug.LogWarning($"{gameObject.name} is currently not carrying any item.");
                return;
            }

            OnDropItem();
            Debug.Log($"Removing up {HeldItem.itemName}");
            HeldItem = null;
        }

        //TODO: handle visualisation of held item -> e.g. instantiate
        protected abstract void OnDropItem();

        public void PickupItem(BaseItem newItem)
        {
            if (HeldItem != null)
            {
                Debug.LogWarning($"{gameObject.name} is already Carrying the item {HeldItem.itemName}! " +
                                 $"You cant pick up {newItem.itemName}!");
                return;
            }

            OnPickupItem();
            Debug.Log($"Picking up {newItem.itemName}");
            HeldItem = newItem;
        }
        
        //TODO: handle visualisation of held item -> e.g. destroy
        protected abstract void OnPickupItem();
    }
}