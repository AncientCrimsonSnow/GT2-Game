using UnityEngine;

namespace Features.TileSystem
{
    public class ResourceTileContext : ITileContext
    {
        public TileContextRegistrator Registrator { get; }

        private readonly string _resourceName;

        public ResourceTileContext(TileContextRegistrator registrator, string resourceName)
        {
            Registrator = registrator;
            _resourceName = resourceName;
        }
        
        public bool OnInteract(GameObject interactor)
        {
            Debug.Log($"Mining {_resourceName}");
            return true;
        }

        public bool IsMovable()
        {
            return false;
        }
    }
}