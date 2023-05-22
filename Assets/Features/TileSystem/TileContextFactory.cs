using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    /// <summary>
    /// Layer for providing Assets (e.g. other Scriptable Objects, Prefabs, primitives, art) for the implemented iTileContext
    /// </summary>
    public abstract class TileContextFactory : ScriptableObject
    {
        public abstract ITileContext Generate(TileContextRegistrator registrator);
    }
}