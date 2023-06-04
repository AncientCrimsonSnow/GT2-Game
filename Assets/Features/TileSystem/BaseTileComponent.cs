using UnityEngine;

namespace Features.TileSystem
{
    public abstract class BaseTileComponent : IInteractable, IMovable, IExchangeable<BaseTileComponent>
    {
        protected readonly Tile Tile;

        protected BaseTileComponent(Tile tile)
        {
            Tile = tile;
        }
        
        public abstract bool TryInteract(GameObject interactor);
        
        public virtual bool IsExchangeable(BaseTileComponent newBaseTileComponent) => true;

        public virtual bool IsMovable() => true;
    }
}