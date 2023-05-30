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
    }
}