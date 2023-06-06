using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public abstract class ItemTileInteractable : ITileInteractable, IExchangeable<ITileInteractable>
    {
        protected readonly Tile.Tile Tile;

        protected ItemTileInteractable(Tile.Tile tile)
        {
            Tile = tile;
        }
        
        public abstract bool TryInteract(GameObject interactor);

        public virtual bool IsMovable() => true;
        
        public virtual bool IsExchangeable(ITileInteractable newTileInteractable)
        {
            return this is EmptyItemTileInteractable || newTileInteractable is EmptyItemTileInteractable;
        }
    }
}