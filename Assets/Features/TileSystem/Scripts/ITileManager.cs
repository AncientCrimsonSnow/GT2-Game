using Unity.Mathematics;

namespace Features.TileSystem.Scripts
{
    public interface ITileManager
    {
        Tile GetTileAt(int2 worldPosition);

        Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize);
    }
}