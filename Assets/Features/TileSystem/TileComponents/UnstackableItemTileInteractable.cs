using Features.TileSystem.CharacterBehaviours;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class UnstackableItemTileInteractable : ItemTileInteractable
    {
        public UnstackableItemTileInteractable(Tile.Tile tile) : base(tile) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor can't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }

            if (heldItemBehaviour.IsCarrying())
            {
                Debug.LogWarning("The interactor can't pickup an item, while carrying one!");
                return false;
            }

            if (!Tile.ItemContainer.ContainsItem() || !Tile.ItemContainer.CanDestroyItem(1))
            {
                Debug.LogError("Either, the UnstackableItemTileComponent doesn't have an Item, even though it should " +
                               "or something with destroying of the current TileContainer failed!");
                return false;
            }

            heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedItem);
            Tile.ItemContainer.DestroyItem(1);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            return true;
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}