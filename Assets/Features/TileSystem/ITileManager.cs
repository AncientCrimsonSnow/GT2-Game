using System.Collections.Generic;
using Unity.Mathematics;

namespace Features.TileSystem
{
    public interface ITileManager
    {
        bool TryGetTileTypeAt<T>(int2 worldPosition, out T tileBase)
            where T : IInteractable, ITileComponentRegistration, IMovable, IGridPosition;

        void RegisterTileContext(BaseTileComponent baseTileComponent, int2 worldPosition);

        void UnregisterTileContext(BaseTileComponent baseTileComponent, int2 worldPosition);

        Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);
    }
}