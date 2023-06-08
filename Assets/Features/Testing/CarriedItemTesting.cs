using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.ItemSystem;
using UnityEngine;

namespace Features.Testing
{
    public class CarriedItemTesting : CarriedItemBaseBehaviour
    {
        [SerializeField] private Item heldItemAtAwake;
        [SerializeField] private bool takeItemAtAwake;
        
        private void Awake()
        {
            if (takeItemAtAwake)
            {
                CarriedItem = heldItemAtAwake;
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