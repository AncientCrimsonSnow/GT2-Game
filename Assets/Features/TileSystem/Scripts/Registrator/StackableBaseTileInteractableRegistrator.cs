using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class StackableBaseTileInteractableRegistrator : BaseTileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem_SO baseItemType;
        [SerializeField] private bool useThisGameObject;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        private bool _canBeUnregistered;

        public override bool CanRegisterTileInteractable()
        {
            _canBeUnregistered = Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() || 
                                 Tile.ContainsTileInteractableOfType<StackableItemTileInteractable>() && Tile.ItemContainer.ContainedBaseItem == baseItemType;
            return _canBeUnregistered;
        }

        public override void RegisterTileInteractable()
        {
            Tile.ItemContainer.AddRegistratorStack();
            
            if (!Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;

            ItemTileInteractable tileComponent = useThisGameObject ?
                new StackableItemTileInteractable(Tile, isMovable, baseItemType, maxContainedItemCount, containedItemAmountOnSpawn, gameObject) 
                : new StackableItemTileInteractable(Tile, isMovable, baseItemType, maxContainedItemCount, containedItemAmountOnSpawn);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        public override bool CanUnregisterTileInteractable()
        {
            return _canBeUnregistered;
        }

        public override void UnregisterTileInteractable()
        {
            Tile.ItemContainer.RemoveRegistratorStack();
            
            if (!Tile.ItemContainer.CanDestroyItem(0)) return;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}