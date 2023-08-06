using Features.Items.Scripts;
using Uilities.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class StackableBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private Poolable poolable;
        [SerializeField] private bool isMovable;
        [SerializeField] private BaseItem_SO containedItemType;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        private bool _canBeUnregistered;

        public override bool CanRegisterOnTile()
        {
            return base.CanRegisterOnTile() && (Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() || 
                                                (Tile.ContainsTileInteractableOfType<StackableItemTileInteractable>() && Tile.ItemContainer.ContainedBaseItem == containedItemType));
        }

        protected override void InternalRegisterOnTile()
        {
            Tile.ItemContainer.AddRegistratorStack();
            
            if (!Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;

            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new StackableItemTileInteractable(Tile, isMovable, containedItemType,
                maxContainedItemCount, containedItemAmountOnSpawn, poolable));
        }

        protected override void InternalUnregisterOnTile()
        {
            Tile.ItemContainer.RemoveRegistratorStack();
            
            if (!Tile.ItemContainer.CanDestroyItem()) return;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}