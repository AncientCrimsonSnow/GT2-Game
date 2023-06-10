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