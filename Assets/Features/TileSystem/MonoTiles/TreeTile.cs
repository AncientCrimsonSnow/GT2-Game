using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem.MonoTiles
{
    public class TreeTile : MonoBehaviour
    {
        private TreeTile tile;
        
        //Astetic stuff
        public int2 MyPos() => tile.MyPos();
    }
}