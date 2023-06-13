using System;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.TileSystem.TileSystem
{
    public static class TileHelper
    {
        public static int2 TransformPositionToInt2(Transform transform)
        {
            var position = transform.position;
            return new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }
        
        public static Vector3Int TransformPositionToVector3Int(Transform transform)
        {
            var position = transform.position;
            return new Vector3Int(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));
        }
        
        public static Tile[,] GetTileKernelAt(Tile[,] tilemap, int2 originArrayPosition, int kernelSize)
        {
            var numRows = tilemap.GetLength(0);
            var numCols = tilemap.GetLength(1);

            var kernelOffset = (kernelSize - 1) / 2;

            var startRow = Math.Max(0, originArrayPosition.x - kernelOffset);
            var endRow = Math.Min(numRows - 1, originArrayPosition.x + kernelOffset);

            var startCol = Math.Max(0, originArrayPosition.y - kernelOffset);
            var endCol = Math.Min(numCols - 1, originArrayPosition.y + kernelOffset);

            Tile[,] neighboringCells = new Tile[endRow - startRow + 1, endCol - startCol + 1];

            for (var x = startRow; x <= endRow; x++)
            {
                for (var y = startCol; y <= endCol; y++)
                {
                    neighboringCells[x - startRow, y - startCol] = tilemap[x, y];
                }
            }

            return neighboringCells;
        }
        
        public static Tile FindClosestCell(Tile[,] tilemap, int2 startPosition, Func<Tile, bool> condition)
        {
            int rows = tilemap.GetLength(0);
            int columns = tilemap.GetLength(1);
            
            int radius = 0;
            int maxRadius = 10;

            while (true)
            {
                if (radius >= maxRadius)
                {
                    Debug.LogError("Couldn't find an empty item slot!");
                    return null;
                }
                
                // Iterate over the top and bottom edges
                for (int x = -radius; x <= radius; x++)
                {
                    int curX = startPosition.x + x;
                    int botY = startPosition.y - radius;
                    int topY = startPosition.y + radius;

                    //bot edge
                    if (IsInBounds(curX, botY, rows, columns) && condition(tilemap[curX, botY])) return tilemap[curX, botY];
                    
                    //top edge
                    if (IsInBounds(curX, topY, rows, columns) && condition(tilemap[curX, topY])) return tilemap[curX, topY];
                }
                
                // Iterate over the left and right edges (excluding corners)
                for (int y = -radius + 1; y < radius; y++)
                {
                    int leftX = startPosition.x - radius;
                    int rightX = startPosition.x + radius;
                    int curY = startPosition.y + y;
                    
                    //left edge
                    if (IsInBounds(leftX, curY, rows, columns) && condition(tilemap[leftX, curY])) return tilemap[leftX, curY];
                    
                    //right edge
                    if (IsInBounds(rightX, curY, rows, columns) && condition(tilemap[rightX, curY])) return tilemap[rightX, curY];
                }

                radius++;
            }
        }

        private static bool IsInBounds(int x, int y, int rows, int columns)
        {
            return x >= 0 && x < rows && y >= 0 && y < columns;
        }
        
        public static int2 WorldToArrayPosition(int2 originPosition, int2 worldPosition)
        {
            return new int2(-originPosition.x + worldPosition.x, -originPosition.y + worldPosition.y);
        }
        
        public static int2 ArrayToWorldPosition(int2 originPosition, int2 arrayPosition)
        {
            return new int2(originPosition.x + arrayPosition.x, originPosition.y + arrayPosition.y);
        }
        
        public static GameObject InstantiateOnTile(IGridPosition worldPosition, GameObject prefab, Quaternion rotation, Transform parent = null)
        {
            var position = new Vector3(worldPosition.WorldPosition.x, 0, worldPosition.WorldPosition.y);
            var instantiateObject = Object.Instantiate(prefab, position, rotation);
            if (parent != null) instantiateObject.transform.SetParent(parent);
            return instantiateObject;
        }
    }
}