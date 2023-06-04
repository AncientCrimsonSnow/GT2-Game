using System;
using UnityEngine;

namespace Features.TileSystem
{
    public class PointerResourceGeneratorTileComponentFactory : TileComponentFactory
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Item item;
        [SerializeField] private int craftAmount;
        
        //TODO: extend new BaseTileComponents & improve this one for design
        public override BaseTileComponent Generate(Tile tile, StackableBaseTileComponent tileComponent)
        {
            var instantiatedObject = TileHelper.InstantiateOnTile(tile, prefab, Quaternion.identity);
            return new PointerResourceGeneratorTileComponent(tile, instantiatedObject, isMovable, tileComponent, item, craftAmount);
        }
    }
}