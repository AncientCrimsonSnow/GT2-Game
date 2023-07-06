using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class UnstackableBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem_SO baseItemType;
    
        public override bool CanRegisterOnTile()
        {
            return base.CanRegisterOnTile() && Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
        }

        protected override void InternalRegisterOnTile()
        {
            ItemTileInteractable tileComponent = new UnstackableItemTileInteractable(Tile, isMovable, baseItemType, gameObject);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override void UnregisterOnTile()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
