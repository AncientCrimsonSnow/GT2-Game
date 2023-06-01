
using Features.TileSystem;
using UnityEngine;

namespace DataStructures.StateLogic
{
    public interface ITileObjectComponent : IActiveInteractable
    {
        void OnUnregister();
        
        //TODO: put somewhere else
        bool IsMovable();

        //TODO: put somewhere else
        bool CanContainObject();
    }
}