using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public abstract class ItemTileInteractable : ITileInteractable, IExchangeable<ITileInteractable>
    {
        protected readonly Tile Tile;
        private readonly bool _isMovable;

        protected ItemTileInteractable(Tile tile, bool isMovable)
        {
            Tile = tile;
            _isMovable = isMovable;
        }
        
        public abstract bool TryInteract(GameObject interactor);

        public bool IsMovable()
        {
            return _isMovable;
        }

        public virtual bool IsExchangeable(ITileInteractable newTileInteractable)
        {
            return this is EmptyItemTileInteractable || newTileInteractable is EmptyItemTileInteractable;
        }
    }
}