using System.Collections.Generic;
using System.Linq;
using Features.Buildings.Scripts;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class ItemCarryBehaviour : BaseItemCarryBehaviour
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private BaseItem_SO heldBaseItemAtAwake;
        [SerializeField] private bool takeItemAtAwake;
        
        private void Awake()
        {
            if (takeItemAtAwake)
            {
                CarriedBaseItem = heldBaseItemAtAwake;
            }
        }

        protected override void OnDropItem()
        {
            var dropKernel = tileManager.GetTileKernelAt(TileHelper.TransformPositionToInt2(transform), 5);
            
            if (!ScriptableObjectByType.TryGetByType(out List<BuildingRecipe> buildingRecipes)) return;

            var buildingOriginPositions = (from Tile tile in dropKernel
                where tile.ItemContainer.ContainsItem() && tile.ItemContainer.ContainedBaseItem.isBuildingOrigin
                select tile.WorldPosition).ToList();

            foreach (var buildingRecipe in buildingRecipes)
            {
                foreach (var buildingOriginPosition in buildingOriginPositions)
                {
                    var buildingOriginKernels = tileManager.GetTileKernelAt(buildingOriginPosition, 3);
                    var recipeKernel = buildingRecipe.GetRecipeKernel();

                    if (IsRecipeFit(buildingOriginKernels, recipeKernel))
                    {
                        //TODO: check by building conditions, if it can be placed
                        //TODO: stacking of some buildings must know, if it can be build, by themself and not by item -> maybe find solution (currently it just works for storages - probably enough RN)
                        Debug.Log("Try Place Building: " + buildingRecipe + " at: " + buildingOriginPosition);
                    }
                }
            }
        }

        private bool IsRecipeFit(Tile[,] tileKernel, BaseItem_SO[,] recipeKernel)
        {
            if (tileKernel.GetLength(0) < 3 || tileKernel.GetLength(1) < 3) return false;
            
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var tile = tileKernel[x, y];

                    if (recipeKernel[x, y] == null) continue;

                    if (!tile.ItemContainer.ContainsItem()) return false;

                    if (tile.ItemContainer.ContainedBaseItem != recipeKernel[x, y]) return false;
                }
            }

            return true;
        }
        
        protected override void OnPickupItem()
        {
        }
    }
}