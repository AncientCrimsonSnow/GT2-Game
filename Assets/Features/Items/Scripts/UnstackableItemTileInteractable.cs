using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class UnstackableItemTileInteractable : ItemTileInteractable
    {
        public UnstackableItemTileInteractable(Tile tile, bool isMovable, BaseItem_SO baseItemType, GameObject useThisGameObject = null) : base(tile, isMovable)
        {
            if (useThisGameObject)
            {
                Tile.ItemContainer.InitializeItem(baseItemType, useThisGameObject);
            }
            else
            {
                Tile.ItemContainer.InitializeItem(baseItemType);
            }
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out BaseItemCarryBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor can't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }

            if (heldItemBehaviour.IsCarrying())
            {
                Debug.LogWarning("The interactor can't pickup an item, while carrying one!");
                return false;
            }

            if (!Tile.ItemContainer.CanDestroyItem(1)) return false;

            heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedBaseItem);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            return true;
        }

        public override bool TryCastMagic(GameObject caster)
        {
            if (!Tile.ItemContainer.CanDestroyItem(1)) return false;
            if (!Tile.ItemContainer.ContainedBaseItem.TryCastMagic(caster)) return false;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            return true;
        }
    }
}