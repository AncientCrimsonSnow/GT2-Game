using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Stackable Resource Tile Context Factory", menuName = "TileContext/Factory/Stackable Resource")]
    public class StackableResourceTileContextFactory : TileContextFactory
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private bool canContainResource;
        
        public override ITileInteractionContext Generate(TileContextRegistrator registrator)
        {
            return new StackableResourceContainer_TileInteractionContext(registrator, isMovable, canContainResource);
        }
    }
}