using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class PickupSingleItemCarryBehaviour : SingleItemCarryBehaviour
    {
        [SerializeField] private BaseItem_SO heldBaseItemAtAwake;
        
        private void Awake()
        {
            if (heldBaseItemAtAwake)
            {
                CarriedBaseItem = heldBaseItemAtAwake;
            }
        }
    }
}