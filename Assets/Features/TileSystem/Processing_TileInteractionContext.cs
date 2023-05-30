using UnityEngine;

namespace Features.TileSystem
{
    public class Processing_TileInteractionContext : BaseTileInteractionContext
    {
        private readonly GameObject _inputTile;
        private readonly string _inputResource;
        private readonly GameObject _outputTile;
        private readonly string _outputResource;
        public TileContextRegistrator Registrator { get; }
        

        public Processing_TileInteractionContext(TileContextRegistrator registrator, bool isMovable, bool canContainResource, 
            GameObject inputTile, string inputResource, GameObject outputTile, string outputResource) 
            : base(registrator, isMovable, canContainResource)
        {
            _inputTile = inputTile;
            _inputResource = inputResource;
            _outputTile = outputTile;
            _outputResource = outputResource;
            Registrator = registrator;
        }
        
        public override bool OnActiveInteract(GameObject interactor)
        {
            return false;
        }
        
        public override void OnPassiveInteract() { }
    }
}