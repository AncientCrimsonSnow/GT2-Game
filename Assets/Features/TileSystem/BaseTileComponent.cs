using UnityEngine;

namespace Features.TileSystem
{
    public abstract class BaseTileComponent : IInteractable, IMovable
    {
        protected readonly Tile Tile;

        protected BaseTileComponent(Tile tile)
        {
            Tile = tile;
        }
        
        public abstract bool TryInteract(GameObject interactor);

        public virtual bool IsMovable() => true;
    }
}