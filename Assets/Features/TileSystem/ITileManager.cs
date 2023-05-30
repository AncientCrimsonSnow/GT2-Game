using System.Collections.Generic;
using Unity.Mathematics;

namespace Features.TileSystem
{
    public interface ITileManager
    {
        public TileBase GetTileAt(int2 worldPosition);

        public void RegisterTileContext(ITileInteractionContext tileInteraction, int2 worldPosition);

        public void UnregisterTileContext(ITileInteractionContext tileInteraction, int2 worldPosition);

        public TileBase[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);
    }
}