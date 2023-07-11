﻿using Features.TileSystem.Scripts;
using Uilities.Pool;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class UnstackableItemTileInteractable : ItemTileInteractable
    {
        public UnstackableItemTileInteractable(Tile tile, bool isMovable, BaseItem_SO baseItemType, Poolable pooledGameObject) : base(tile, isMovable)
        {
            Tile.ItemContainer.InitializeItem(baseItemType, pooledGameObject);
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IItemCarryBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor can't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }

            if (!Tile.ItemContainer.CanDestroyItem() || !heldItemBehaviour.CanCarryMore()) return false;

            heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedBaseItem);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
            return true;
        }

        public override bool TryCast(GameObject caster)
        {
            return Tile.ItemContainer.ContainedBaseItem.TryCast(caster);
        }
    }
}