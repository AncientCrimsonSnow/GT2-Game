﻿using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public class TileManager : ITileManager
    {
        private readonly int2 _mapOrigin;
        private readonly TileBase[,] _tileMap;

        //positions currently expects, that it doesnt use negative values
        public TileManager(int2 mapSize, int2 mapOrigin)
        {
            _mapOrigin = mapOrigin;
            _tileMap = new TileBase[mapSize.x, mapSize.y];

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    _tileMap[x, y] = new TileBase(new int2(_mapOrigin.x + x, _mapOrigin.y + y));
                }
            }
        }

        private int2 WorldToArrayPosition(int2 worldPosition)
        {
            return new int2(-_mapOrigin.x + worldPosition.x, -_mapOrigin.y + worldPosition.y);
        }
        
        public TileBase GetTileAt(int2 worldPosition)
        {
            var arrayPosition = WorldToArrayPosition(worldPosition);
            
            return _tileMap[arrayPosition.x, arrayPosition.y];
        }

        /// <summary>
        /// Will always be called by the TileContentRegistrator after it got instantiated
        /// </summary>
        /// <param name="tileInteraction"></param>
        /// <param name="worldPosition"></param>
        /// <param name="tileContextType"></param>
        public void RegisterTileContext(ITileInteractionContext tileInteraction, int2 worldPosition)
        {
            var arrayPosition = WorldToArrayPosition(worldPosition);
            
            _tileMap[arrayPosition.x, arrayPosition.y].RegisterTileContext(tileInteraction);
        }
        
        /// <summary>
        /// Will always be called by the TileContentRegistrator after it got destroyed, in case it doesn't got cleaned up earlier.
        /// </summary>
        /// <param name="tileInteraction"></param>
        /// <param name="worldPosition"></param>
        /// <param name="tileContextType"></param>
        public void UnregisterTileContext(ITileInteractionContext tileInteraction, int2 worldPosition)
        {
            var arrayPosition = WorldToArrayPosition(worldPosition);
            
            _tileMap[arrayPosition.x, arrayPosition.y].UnregisterTileContext(tileInteraction);
        }
        
        public TileBase[,] GetTileKernelAt(int2 originWorldPosition, int kernelSize)
        {
            int2 arrayPosition = WorldToArrayPosition(originWorldPosition);

            return TileHelper.GetTileKernelAt(_tileMap, arrayPosition, kernelSize);
        }
    }
}