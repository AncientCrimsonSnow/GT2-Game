using Features.ReplaySystem.Record;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Character.Scripts.Interaction
{
    public class InteractionInputSnapshot : IInputSnapshot
    {
        private readonly GameObject _interactor;
        private readonly TileManager _tileManager;

        public InteractionInputSnapshot(GameObject interactor, TileManager tileManager)
        {
            _interactor = interactor;
            _tileManager = tileManager;
        }
    
        public void Tick(float tickDurationInSeconds)
        {
            var registeredPosition = TileHelper.TransformPositionToInt2(_interactor.transform);
            var tile = _tileManager.GetTileAt(registeredPosition);
            tile.TryInteract(_interactor);
        }
    }
}
