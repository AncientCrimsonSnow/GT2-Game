using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public class TileManager : ITileManager
    {
        private readonly int2 _mapOrigin;
        private readonly Tile[,] _tileMap;

        //positions currently expects, that it doesnt use negative values
        public TileManager(int2 mapSize, int2 mapOrigin)
        {
            _mapOrigin = mapOrigin;
            _tileMap = new Tile[mapSize.x, mapSize.y];

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    var arrayPosition = new int2(x, y);
                    _tileMap[x, y] = new Tile(TileHelper.ArrayToWorldPosition(_mapOrigin, new int2(x, y)), arrayPosition);
                }
            }
        }

        public Tile GetTileTypeAt(int2 worldPosition)
        {
            var arrayPosition = TileHelper.WorldToArrayPosition(_mapOrigin, worldPosition);

            return _tileMap[arrayPosition.x, arrayPosition.y];
        }
        
        public Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize)
        {
            var arrayPosition = TileHelper.WorldToArrayPosition(_mapOrigin, originWorldPosition);

            return TileHelper.GetTileKernelAt(_tileMap, arrayPosition, kernelSize);
        }
    }
}