using Unity.VisualScripting;
using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableResourceContainerTileInteractionContext : BaseTileInteractionContext
    {
        public TileContextRegistrator Registrator { get; }

        public UnstackableResourceContainerTileInteractionContext(TileContextRegistrator registrator, bool isMovable, bool canContainResource) 
            : base(registrator, isMovable, canContainResource)
        {
            Registrator = registrator;
        }
        
        public override bool OnActiveInteract(GameObject interactor)
        {
            Debug.Log($"Picking up {_resourceName}");
            Object.Destroy(Registrator.gameObject);
            
            return true;
        }
        
        public override void OnPassiveInteract() { }
    }
}