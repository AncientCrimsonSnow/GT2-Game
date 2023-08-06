using Features.Items.Scripts;
using Uilities.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class UnstackableBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private Poolable poolable;
        [SerializeField] private bool isMovable;
        [SerializeField] private BaseItem_SO containedItemType;
    
        public override bool CanRegisterOnTile()
        {
            //Tile.PrintInteractable();
            return base.CanRegisterOnTile() && Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
        }

        protected override void InternalRegisterOnTile()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(Tile, isMovable, containedItemType, poolable));
        }

        protected override void InternalUnregisterOnTile()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
