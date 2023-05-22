using System.Collections.Generic;
using Unity.Mathematics;

namespace Features.TileSystem
{
    public interface ITileManager
    {
        public TileBase GetTileAt(int2 worldPosition);

        public void RegisterTileContext(ITileContext tile, int2 worldPosition, TileContextType tileContextType);

        public void UnregisterTileContext(ITileContext tile, int2 worldPosition);

        public TileBase[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);

        public List<TileBase> GetCardinalNeighboringCells(int2 originWorldPosition, int depth);
    }
}