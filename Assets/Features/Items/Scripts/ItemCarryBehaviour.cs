using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class ItemCarryBehaviour : BaseItemCarryBehaviour
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private BaseItem_SO heldBaseItemAtAwake;
        
        private void Awake()
        {
            if (heldBaseItemAtAwake)
            {
                CarriedBaseItem = heldBaseItemAtAwake;
            }
        }

        private void OnDestroy()
        {
            if (CarriedBaseItem != null)
            {
                TileHelper.DropItemNearestEmptyTile(tileManager, transform.position, CarriedBaseItem);
            }
        }

        protected override void OnDropItem()
        {
        }

        protected override void OnPickupItem()
        {
        }
    }
}