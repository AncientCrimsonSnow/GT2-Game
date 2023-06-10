using Unity.Mathematics;

namespace Features.TileSystem.TileSystem
{
    public interface ITileManager
    {
        Tile GetTileAt(int2 worldPosition);

        Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);
    }
}