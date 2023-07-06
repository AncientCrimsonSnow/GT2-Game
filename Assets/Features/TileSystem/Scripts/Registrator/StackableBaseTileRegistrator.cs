using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class StackableBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem_SO baseItemType;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        private bool _canBeUnregistered;

        public override bool CanRegisterOnTile()
        {
            return base.CanRegisterOnTile() && (Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() || 
                                                (Tile.ContainsTileInteractableOfType<StackableItemTileInteractable>() && Tile.ItemContainer.ContainedBaseItem == baseItemType));
        }

        protected override void InternalRegisterOnTile()
        {
            Tile.ItemContainer.AddRegistratorStack();
            
            if (!Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;

            ItemTileInteractable tileComponent = new StackableItemTileInteractable(Tile, isMovable, baseItemType, maxContainedItemCount, containedItemAmountOnSpawn, gameObject);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override void UnregisterOnTile()
        {
            Tile.ItemContainer.RemoveRegistratorStack();
            
            if (!Tile.ItemContainer.CanDestroyItem(0)) return;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}