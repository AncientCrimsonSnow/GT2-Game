using UnityEngine;

namespace Features.TileSystem
{
    public abstract class BaseTileInteractionContext : ITileInteractionContext
    {
        public TileContextRegistrator Registrator { get; }
        
        private readonly bool _isMovable;
        private readonly bool _canContainResource;

        public BaseTileInteractionContext(TileContextRegistrator registrator, bool isMovable, bool canContainResource)
        {
            _isMovable = isMovable;
            _canContainResource = canContainResource;
            Registrator = registrator;
        }

        public abstract bool OnActiveInteract(GameObject interactor);

        public abstract void OnPassiveInteract();
        
        public bool IsMovable()
        {
            return _isMovable;
        }

        public bool CanContainResource()
        {
            return _canContainResource;
        }
    }
}