using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Features.TileSystem.MonoTiles
{
    public class TreeTile : MonoBehaviour
    {
        private TreeTile tile;
        
        //Astetic stuff
        public int2 MyPos() => tile.MyPos();

        
        [Inject]
        public void Construct(ITileManager TEST)
        {
                
        }
    }
}