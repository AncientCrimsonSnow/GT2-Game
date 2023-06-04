using System.Collections.Generic;
using Unity.Mathematics;

namespace Features.TileSystem
{
    public interface ITileManager
    {
        Tile GetTileTypeAt(int2 worldPosition);

        void RegisterTileContext(BaseTileComponent baseTileComponent, int2 worldPosition);

        void UnregisterTileContext(BaseTileComponent baseTileComponent, int2 worldPosition);

        Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);
    }
}