using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class SingleItemCarryBehaviour : MonoBehaviour, IItemCarryBehaviour
    {
        [SerializeField] private TileManager tileManager;

        protected BaseItem_SO CarriedBaseItem;
        
        public BaseItem_SO GetNextCarried() => CarriedBaseItem;
        public bool IsCarrying() => CarriedBaseItem != null;
        public bool CanCarryMore() => CarriedBaseItem == null;
        
        private void OnDestroy()
        {
            if (CarriedBaseItem != null)
            {
                TileHelper.DropItemNearestEmptyTile(tileManager, transform.position, CarriedBaseItem);
            }
        }

        public void DropItem(BaseItem_SO droppedItemType)
        {
            if (CarriedBaseItem == null)
            {
                Debug.LogWarning($"{gameObject.name} is currently not carrying any item.");
                return;
            }

            if (CarriedBaseItem != droppedItemType)
            {
                Debug.LogWarning($"{droppedItemType} is not carried!");
                return;
            }
            
            Debug.Log("Drop");
            CarriedBaseItem = null;
        }

        public void PickupItem(BaseItem_SO newBaseItem)
        {
            if (CarriedBaseItem != null)
            {
                Debug.LogWarning($"{gameObject.name} is already Carrying the item {CarriedBaseItem.itemName}! " +
                                 $"You cant pick up {newBaseItem.itemName}!");
                return;
            }

            Debug.Log("Pickup");
            CarriedBaseItem = newBaseItem;
        }
    }
}