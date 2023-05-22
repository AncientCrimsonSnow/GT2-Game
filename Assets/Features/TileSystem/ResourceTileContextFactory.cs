using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Resource Tile Context Factory", menuName = "TileContext/Factory/Resource")]
    public class ResourceTileContextFactory : TileContextFactory
    {
        [SerializeField] private string resourceName;
        
        public override ITileContext Generate(TileContextRegistrator registrator)
        {
            return new ResourceTileContext(registrator, resourceName);
        }
    }
}