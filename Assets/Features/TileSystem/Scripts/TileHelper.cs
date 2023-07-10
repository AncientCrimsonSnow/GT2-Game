using System;
using Features.Items.Scripts;
using Uilities.Pool;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.TileSystem.Scripts
{
    public static class TileHelper
    {
        public static int2 TransformPositionToInt2(Transform transform)
        {
            return Vector3ToInt2(transform.position);
        }
        
        public static int2 Vector3ToInt2(Vector3 position)
        {
            return new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }
        
        public static Vector3Int TransformPositionToVector3Int(Transform transform)
        {
            var position = transform.position;
            return new Vector3Int(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));
        }
        
        public static T[,] GetTileKernelAt<T>(T[,] tilemap, int2 originArrayPosition, int kernelSize)
        {
            var numRows = tilemap.GetLength(0);
            var numCols = tilemap.GetLength(1);

            var kernelOffset = (kernelSize - 1) / 2;

            var startRow = Math.Max(0, originArrayPosition.x - kernelOffset);
            var endRow = Math.Min(numRows - 1, originArrayPosition.x + kernelOffset);

            var startCol = Math.Max(0, originArrayPosition.y - kernelOffset);
            var endCol = Math.Min(numCols - 1, originArrayPosition.y + kernelOffset);

            T[,] neighboringCells = new T[endRow - startRow + 1, endCol - startCol + 1];

            for (var x = startRow; x <= endRow; x++)
            {
                for (var y = startCol; y <= endCol; y++)
                {
                    neighboringCells[x - startRow, y - startCol] = tilemap[x, y];
                }
            }

            return neighboringCells;
        }
        
        public static T[,] RotateKernelClockwise<T>(T[,] kernel)
        {
            int rows = kernel.GetLength(0);
            int cols = kernel.GetLength(1);
            T[,] rotatedKernel = new T[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotatedKernel[j, rows - 1 - i] = kernel[i, j];
                }
            }

            return rotatedKernel;
        }
        
        public static T FindClosestCell<T>(T[,] tilemap, int2 startPosition, Func<T, bool> condition)
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
                    return default;
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
        
        public static Poolable ReuseOnTile(IGridPosition worldPosition, GameObject prefab, Quaternion rotation, Transform parent = null)
        {
            var position = new Vector3(worldPosition.WorldPosition.x, 0, worldPosition.WorldPosition.y);
            var poolable = parent ? prefab.Reuse(position, rotation, parent) : prefab.Reuse(position, rotation);
            return poolable;
        }
        
        public static void DropItemNearestEmptyTile(TileManager tileManager, Vector3 position, BaseItem_SO baseItem)
        {
            var worldPositionInt2 = Vector3ToInt2(position);
            var foundTile = tileManager.SearchNearestTileByCondition(worldPositionInt2,
                tile => tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() ||
                        (tile.TryGetFirstTileInteractableOfType(out StackableItemTileInteractable _) &&
                         tile.ItemContainer.IsItemFit(baseItem) && tile.ItemContainer.CanAddItemCount(1)));

            if (foundTile.ContainsTileInteractableOfType<EmptyItemTileInteractable>())
            {
                ReuseOnTile(foundTile, baseItem.prefab, Quaternion.identity);
            }
            else
            {
                foundTile.ItemContainer.AddItemCount(baseItem, 1);
            }
        }
    }
}