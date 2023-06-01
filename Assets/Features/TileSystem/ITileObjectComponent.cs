
using Features.TileSystem;
using UnityEngine;

namespace DataStructures.StateLogic
{
    public interface ITileObjectComponent : IActiveInteractable
    {
        bool IsMovable();

        bool CanContainObject();
    }
}