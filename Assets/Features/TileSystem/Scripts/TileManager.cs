using System;
using Features.TileSystem.TileComponents;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem.TileSystem
{
    [CreateAssetMenu(fileName = "new TileManager", menuName = "TileManager")]
    public class TileManager : ScriptableObject, ITileManager
    {
        [SerializeField] private int2 mapSize;
        [SerializeField] private int2 mapOrigin;
        
        private int2 _mapOrigin;
        private Tile[,] _tileMap;

        private bool _isInitialized;

        private void OnEnable()
        {
            _isInitialized = false;
        }

        public Tile GetTileAt(int2 worldPosition)
        {
            Setup();
            
            var arrayPosition = TileHelper.WorldToArrayPosition(_mapOrigin, worldPosition);

            return _tileMap[arrayPosition.x, arrayPosition.y];
        }
        
        public Tile[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize)
        {
            Setup();
            
            var arrayPosition = TileHelper.WorldToArrayPosition(_mapOrigin, originWorldPosition);

            return TileHelper.GetTileKernelAt(_tileMap, arrayPosition, kernelSize);
        }

        public Tile SearchNearestTileByCondition(int2 worldPosition, Func<Tile, bool> condition)
        {
            var arrayPosition = TileHelper.WorldToArrayPosition(_mapOrigin, worldPosition);
            return TileHelper.FindClosestCell(_tileMap, arrayPosition, condition);
        }

        private void Setup()
        {
            if (_isInitialized) return;
            
            Initialize();
            _isInitialized = true;
        }
        
        //positions currently expects, that it doesnt use negative values
        private void Initialize()
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
    }
}