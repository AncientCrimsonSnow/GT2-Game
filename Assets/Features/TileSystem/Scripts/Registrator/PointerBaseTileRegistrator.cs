using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class PointerBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemLoot")] [SerializeField] private BaseItem_SO baseItemLoot;

        [FormerlySerializedAs("pointerRegistrator")] [SerializeField] private List<BaseTileRegistrator> pointerRegistrators;
        [SerializeField] private int craftAmount;

        private ITileInteractable _tileInteractable;
        
        public override bool CanRegisterOnTile()
        {
            return base.CanRegisterOnTile() && pointerRegistrators.All(x => x.CanRegisterOnTile());
        }

        protected override void InternalRegisterOnTile()
        {
            var tileComponent = new PointerResourceGeneratorTileInteractable(Tile, isMovable, pointerRegistrators, baseItemLoot, craftAmount);
            Tile.RegisterTileInteractable(tileComponent);
            _tileInteractable = tileComponent;
        }

        protected override void InternalUnregisterOnTile()
        {
            Tile.UnregisterTileInteractable(_tileInteractable);
        }
    }
}