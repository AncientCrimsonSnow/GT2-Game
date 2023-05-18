using Features.Interactors;
using Unity.Mathematics;

namespace Features.TileSystem.UnmovableTile
{
    public abstract class BlockedTile : IUnmovableTile
    {
        public int2 Position { get; set; }
        
        public void Interact(ITile tile, IInteractor interactor)
        {
            
        }
    }
}