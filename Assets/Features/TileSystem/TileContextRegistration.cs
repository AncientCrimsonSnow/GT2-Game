using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class TileContextRegistration : ScriptableObject
    {
        public abstract void OnRegister(ITileInteractionContext ownedTileInteractionContext, ITileManager tileManager, int2 worldPosition);

        public abstract void OnUnregister(ITileInteractionContext ownedTileInteractionContext, ITileManager tileManager, int2 worldPosition);
    }
}