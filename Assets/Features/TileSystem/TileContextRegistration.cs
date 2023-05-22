using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    public abstract class TileContextRegistration : ScriptableObject
    {
        public abstract void OnRegister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition);

        public abstract void OnUnregister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition);
    }
}