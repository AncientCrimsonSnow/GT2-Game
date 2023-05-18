using Unity.Mathematics;
using Unity.VisualScripting;

namespace Features.TileSystem
{
    public class TileManager : ITileManager
    {
        private ITile[,] _tileMap;

        public TileManager(int2 mapSize)
        {
            _tileMap = new ITile[mapSize.x, mapSize.y];
            for(var y = 0; y != mapSize.y; y++)
            {
                for (var x = 0; x != mapSize.x; x++)
                {
                    //Some kind of map Gen
                    _tileMap[y,x] = GetRandomTile();
                }
            }
        }

        private ITile GetRandomTile()
        {
            throw new System.NotImplementedException();
        }
    }
}