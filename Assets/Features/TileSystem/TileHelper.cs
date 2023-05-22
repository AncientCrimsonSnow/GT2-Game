using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public static class TileHelper
    {
        public static int2 TransformPositionToInt2(Transform transform)
        {
            var position = transform.position;
            return new int2((int)position.x, (int)position.z);
        }
        
        public static TileBase[,] GetTileKernelAt(TileBase[,] tilemap, int2 originArrayPosition, int kernelSize)
        {
            int numRows = tilemap.GetLength(0);
            int numCols = tilemap.GetLength(1);

            int kernelOffset = (kernelSize - 1) / 2;

            int startRow = Math.Max(0, originArrayPosition.x - kernelOffset);
            int endRow = Math.Min(numRows - 1, originArrayPosition.x + kernelOffset);

            int startCol = Math.Max(0, originArrayPosition.y - kernelOffset);
            int endCol = Math.Min(numCols - 1, originArrayPosition.y + kernelOffset);

            TileBase[,] neighboringCells = new TileBase[endRow - startRow + 1, endCol - startCol + 1];

            for (int x = startRow; x <= endRow; x++)
            {
                for (int y = startCol; y <= endCol; y++)
                {
                    neighboringCells[x - startRow, y - startCol] = tilemap[x, y];
                }
            }

            return neighboringCells;
        }

        public static bool IsNeighborInsideDepth(int iterator, int originPosition, int depth) => 
            Mathf.Abs(iterator - originPosition) <= depth && Mathf.Abs(iterator - originPosition) > 0;
        
        public static bool IsInBounds(TileBase[,] grid, int position) => position >= 0 && position < grid.GetLength(1);
        
        
        public static List<TileBase> GetCardinalNeighboringCells(TileBase[,] tilemap, int2 originArrayPosition, int depth)
        {
            var result = new List<TileBase>();
            
            for (var x = 0; x < tilemap.GetLength(0); x++)
            {
                for (var y = 0; y < tilemap.GetLength(1); y++)
                {
                    if (x == originArrayPosition.x && IsInBounds(tilemap, originArrayPosition.y) && IsNeighborInsideDepth(y, originArrayPosition.y, depth)|| 
                        y == originArrayPosition.y && IsInBounds(tilemap, originArrayPosition.x) && IsNeighborInsideDepth(x, originArrayPosition.x, depth))
                    {
                        result.Add(tilemap[x, y]);
                    }
                }
            }

            return result;
        }
    }
}