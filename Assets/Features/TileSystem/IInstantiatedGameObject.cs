using UnityEngine;

namespace Features.TileSystem
{
    /// <summary>
    /// Layer for getting components from the GameObject, that the TileContextRegistrator Component is attached to.
    /// Can be extended into Service Provider, when it has many components.
    /// </summary>
    public interface IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; }
    }
}