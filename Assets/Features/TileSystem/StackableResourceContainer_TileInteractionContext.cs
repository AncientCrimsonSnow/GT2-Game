using UnityEngine;

namespace Features.TileSystem
{
    public class StackableResourceContainer_TileInteractionContext : BaseTileInteractionContext
    {
        public TileContextRegistrator Registrator { get; }

        private int _itemCount;

        public StackableResourceContainer_TileInteractionContext(TileContextRegistrator registrator, bool isMovable, bool canContainResource) 
            : base(registrator, isMovable, canContainResource)
        {
            Registrator = registrator;
        }
        
        public override bool OnActiveInteract(GameObject interactor)
        {
            if (_itemCount == 0) return false;
            
            _itemCount--;
            Debug.Log($"Picking up {_resourceName}");
            
            return true;
        }
        
        public override void OnPassiveInteract() { }
    }
}