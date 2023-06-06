using UnityEngine;

namespace Features.TileSystem.CharacterBehaviours
{
    public abstract class CarriedItemBaseBehaviour : MonoBehaviour
    {
        public Item.Item CarriedItem { get; protected set; }
        
        public bool IsCarrying() => CarriedItem != null;

        public void DropItem()
        {
            if (CarriedItem == null)
            {
                Debug.LogWarning($"{gameObject.name} is currently not carrying any item.");
                return;
            }

            OnDropItem();
            Debug.Log("Drop");
            CarriedItem = null;
        }

        //TODO: handle visualisation of held item -> e.g. instantiate
        protected abstract void OnDropItem();

        public void PickupItem(Item.Item newItem)
        {
            if (CarriedItem != null)
            {
                Debug.LogWarning($"{gameObject.name} is already Carrying the item {CarriedItem.itemName}! " +
                                 $"You cant pick up {newItem.itemName}!");
                return;
            }

            OnPickupItem();
            Debug.Log("Pickup");
            CarriedItem = newItem;
        }
        
        //TODO: handle visualisation of held item -> e.g. destroy
        protected abstract void OnPickupItem();
    }
}