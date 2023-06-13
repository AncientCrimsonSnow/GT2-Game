using UnityEngine;

namespace Features.Items.Scripts
{
    public abstract class BaseItemCarryBehaviour : MonoBehaviour
    {
        public BaseItem_SO CarriedBaseItem { get; protected set; }
        
        public bool IsCarrying() => CarriedBaseItem != null;

        public void DropItem()
        {
            if (CarriedBaseItem == null)
            {
                Debug.LogWarning($"{gameObject.name} is currently not carrying any item.");
                return;
            }

            OnDropItem();
            Debug.Log("Drop");
            CarriedBaseItem = null;
        }

        //TODO: handle visualisation of held item -> e.g. instantiate
        protected abstract void OnDropItem();

        public void PickupItem(BaseItem_SO newBaseItem)
        {
            if (CarriedBaseItem != null)
            {
                Debug.LogWarning($"{gameObject.name} is already Carrying the item {CarriedBaseItem.itemName}! " +
                                 $"You cant pick up {newBaseItem.itemName}!");
                return;
            }

            OnPickupItem();
            Debug.Log("Pickup");
            CarriedBaseItem = newBaseItem;
        }
        
        //TODO: handle visualisation of held item -> e.g. destroy
        protected abstract void OnPickupItem();
    }
}