using Features.Interactors;
using Features.TileSystem.Interactables;
using Unity.Mathematics;

namespace Features.TileSystem.UnmovableTile
{
    public abstract class ResourceTile : IUnmovableTile
    {
        private readonly ITileManager _tileManager;
        
        public int2 Position { get; set; }
        protected ResourceTile(ITileManager tileManager)
        {
            _tileManager = tileManager;
        }
        
        
        public void Interact(ITile tile , IInteractor interactor)
        {
            var resource = GetResource();
            
            
        }

        protected abstract Resource GetResource();
        
        
    }
}