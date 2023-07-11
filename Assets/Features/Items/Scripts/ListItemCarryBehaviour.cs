using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public enum ItemCarryRemoveType { Stack, Queue }
    public class ListItemCarryBehaviour : MonoBehaviour, IItemCarryBehaviour
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private ItemCarryRemoveType itemCarryRemoveType;
        [SerializeField] private int maxSimultaneouslyCarriedItems;
    
        public bool IsCarrying() => _carriedBaseItem.Count > 0;
        public bool CanCarryMore() => _carriedBaseItem.Count < maxSimultaneouslyCarriedItems;
        public BaseItem_SO GetNextCarried() => itemCarryRemoveType == ItemCarryRemoveType.Stack ? _carriedBaseItem[^1] : _carriedBaseItem[0];

    
        private List<BaseItem_SO> _carriedBaseItem;

        private void Awake()
        {
            _carriedBaseItem = new List<BaseItem_SO>();
        }
    
        private void OnDestroy()
        {
            foreach (var baseItem in _carriedBaseItem)
            {
                TileHelper.DropItemNearestEmptyTile(tileManager, transform.position, baseItem);
            }
        }

        public void DropItem(BaseItem_SO droppedItemType)
        {
            if (!IsCarrying())
            {
                Debug.LogWarning($"{gameObject.name} is currently not carrying any item.");
                return;
            }

            Debug.Log("Drop");
            _carriedBaseItem.Remove(droppedItemType);
        }

        public void PickupItem(BaseItem_SO newBaseItem)
        {
            if (!CanCarryMore())
            {
                Debug.LogWarning($"{gameObject.name} is already Carrying {_carriedBaseItem.Count} items! You cant pick up more!");
                return;
            }

            Debug.Log("Pickup");
            _carriedBaseItem.Add(newBaseItem);
        }
    }
}