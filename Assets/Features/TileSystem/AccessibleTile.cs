using Features.Interactors;
using Unity.Mathematics;

namespace Features.TileSystem
{
    public class AccessibleTile : ITile
    {
        private readonly IUnmovableTile _unmovableNeighbourTile;
        
        public int2 Position { get; set; }
        
        private IInteract _interact;
        
        public AccessibleTile(
            IUnmovableTile unmovableNeighbourTile)
        {
            _unmovableNeighbourTile = unmovableNeighbourTile;
        }

        public void Interact(IInteractor interactor)
        {
            if (_interact.Interact())
            {
                //TODO WAS IN DER DER HAND ABLeGEN
                
                //wenn nichts inder hand weiter
                _unmovableNeighbourTile.Interact(this, interactor);
            }
        }
        

        public void SetTileInteractable(IInteract interact)
        {
            _interact = interact;
        }
    }
}