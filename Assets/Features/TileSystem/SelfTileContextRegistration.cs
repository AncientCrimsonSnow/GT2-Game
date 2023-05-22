using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Self Tile Context Registrator", menuName = "TileContext/Registrator/Self")]
    public class SelfTileContextRegistration : TileContextRegistration
    {
        public override void OnRegister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition)
        {
            tileManager.RegisterTileContext(ownedTileContext, worldPosition, TileContextType.SelfContext);
        }

        public override void OnUnregister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition)
        {
            var tilePosition = worldPosition;
            var tileBase = tileManager.GetTileAt(tilePosition);
            if (tileBase.HasTileContextOfType(TileContextType.SelfContext) && ownedTileContext != null)
            {
                tileManager.UnregisterTileContext(ownedTileContext, tilePosition);
            }
        }
    }
}