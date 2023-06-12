using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class EmptyItemTileInteractable : ItemTileInteractable
    {
        public EmptyItemTileInteractable(Tile tile) : base(tile, true)
        {
            Tile.ItemContainer.DestroyItem(1);
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor cant't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }

            if (!heldItemBehaviour.IsCarrying())
            {
                Debug.LogWarning("The Interactor isn't carrying an item!");
                return false;
            }

            var heldItem = heldItemBehaviour.CarriedBaseItem;
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(Tile, IsMovable(), heldItem));
            heldItemBehaviour.DropItem();
            return true;
        }

        public override bool TryCastMagic(GameObject caster)
        {
            return false;
        }
    }
}