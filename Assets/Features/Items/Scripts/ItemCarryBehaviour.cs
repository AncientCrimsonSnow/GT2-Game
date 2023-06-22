using System.Collections.Generic;
using System.Linq;
using Features.Buildings.Scripts;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class ItemCarryBehaviour : BaseItemCarryBehaviour
    {
        [SerializeField] private BaseItem_SO heldBaseItemAtAwake;
        [SerializeField] private bool takeItemAtAwake;
        
        private void Awake()
        {
            if (takeItemAtAwake)
            {
                CarriedBaseItem = heldBaseItemAtAwake;
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