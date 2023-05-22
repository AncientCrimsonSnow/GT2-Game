using Unity.Mathematics;
using UnityEngine;

namespace Features.TileSystem
{
    [CreateAssetMenu(fileName = "Pointer Tile Context Registrator", menuName = "TileContext/Registrator/Pointer")]
    public class PointerTileContextRegistration : SelfTileContextRegistration
    {
        [SerializeField] private int kernelSize;
        
        public override void OnRegister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition)
        {
            base.OnRegister(ownedTileContext, tileManager, worldPosition);

            foreach (var cardinalNeighboringCell in tileManager.GetCardinalNeighboringCells(worldPosition, kernelSize))
            {
                cardinalNeighboringCell.RegisterTileContext(ownedTileContext, TileContextType.PointerContext);
            }
        }

        public override void OnUnregister(ITileContext ownedTileContext, ITileManager tileManager, int2 worldPosition)
        {
            base.OnUnregister(ownedTileContext, tileManager, worldPosition);
            
            foreach (var cardinalNeighboringCell in tileManager.GetCardinalNeighboringCells(worldPosition, kernelSize))
            {
                cardinalNeighboringCell.UnregisterTileContext(ownedTileContext);
            }
        }
    }
}