using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class EmptyItemTileInteractable : ItemTileInteractable
    {
        public EmptyItemTileInteractable(Tile tile) : base(tile, true)
        {
            Tile.ItemContainer.DestroyItem(1);
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out BaseItemCarryBehaviour heldItemBehaviour))
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
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            TileHelper.InstantiateOnTile(Tile, heldItem.prefab, Quaternion.identity);
            heldItemBehaviour.DropItem();
            return true;
        }

        public override bool TryCast(GameObject caster)
        {
            return false;
        }
    }
}