using Unity.VisualScripting;
using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableItemContainer : ItemContainerBase
    {
        public UnstackableItemContainer(string itemName, GameObject instantiatedObject) 
            : base(itemName, instantiatedObject) { }
        
        public override bool OnActiveInteract(GameObject interactor)
        {
            Debug.Log($"Picking up {ItemName}");
            
            Object.Destroy(InstantiatedObject);
            //TODO: remove from TileBase on destroy
            
            return true;
        }
    }
}