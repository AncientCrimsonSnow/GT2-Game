using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class UnstackableItemTileInteractable : ItemTileInteractable
    {
        public UnstackableItemTileInteractable(Tile tile, bool isMovable, Item itemType, GameObject useThisGameObject = null) : base(tile, isMovable)
        {
            if (useThisGameObject)
            {
                Tile.ItemContainer.InitializeItem(itemType, useThisGameObject);
            }
            else
            {
                Tile.ItemContainer.InitializeItem(itemType);
            }
        }

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

            if (Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() || !Tile.ItemContainer.CanDestroyItem(1))
            {
                Debug.LogError("Either, the UnstackableItemTileComponent doesn't have an Item, even though it should " +
                               "or something with destroying of the current TileContainer failed!");
                return false;
            }

            heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedItem);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            return true;
        }
    }
}