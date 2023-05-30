using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Self Tile Context Registrator", menuName = "TileContext/Registrator/Self")]
    public class SelfTileContextRegistration : TileContextRegistration
    {
        public override void OnRegister(ITileInteractionContext ownedTileInteractionContext, ITileManager tileManager, int2 worldPosition)
        {
            tileManager.RegisterTileContext(ownedTileInteractionContext, worldPosition);
        }

        public override void OnUnregister(ITileInteractionContext ownedTileInteractionContext, ITileManager tileManager, int2 worldPosition)
        {
            var tilePosition = worldPosition;
            var tileBase = tileManager.GetTileAt(tilePosition);
            if (tileBase.HasTileContext() && ownedTileInteractionContext != null)
            {
                tileManager.UnregisterTileContext(ownedTileInteractionContext, tilePosition);
            }
        }
    }
}