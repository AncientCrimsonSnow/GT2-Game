using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Items.Scripts
{
    public class ItemCarryBehaviour : BaseItemCarryBehaviour
    {
        [FormerlySerializedAs("heldItemAtAwake")] [SerializeField] private BaseItem_SO heldBaseItemAtAwake;
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