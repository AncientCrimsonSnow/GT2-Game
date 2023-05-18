using Features.Interactors;

namespace Features.TileSystem
{
    public interface IUnmovableTile : ITile
    {
        public void Interact(ITile tile, IInteractor interactor);
    }
}