using UnityEngine;

namespace Features.TileSystem
{
    public abstract class ItemTileComponent : ITileComponent, IExchangeable<ITileComponent>
    {
        protected readonly Tile Tile;

        protected ItemTileComponent(Tile tile)
        {
            Tile = tile;
        }
        
        public abstract bool TryInteract(GameObject interactor);

        public virtual bool IsMovable() => true;
        
        public virtual bool IsExchangeable(ITileComponent newTileComponent)
        {
            return newTileComponent is EmptyItemTileComponent;
        }
    }
}