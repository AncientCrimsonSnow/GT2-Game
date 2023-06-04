using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class TileComponentFactory : ScriptableObject
    {
        public abstract BaseTileComponent Generate(Tile tile, StackableItemTileComponent tileComponent);
    }
}