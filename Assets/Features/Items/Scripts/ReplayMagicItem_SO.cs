using System;
using System.Collections.Generic;
using System.Linq;
using Features.Buildings.Scripts;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Magic;
using Features.Character.Scripts.Movement;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class ReplayMagicItem_SO : BaseItem_SO, IDirectionInput, ICastInput, IInteractInput
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private int kernelSize = 3;

        [SerializeField] private DirectionInputFocus directionInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private CastInputFocus castInputFocus;

        
        private BuildSequenceStateMachine _buildSequenceStateMachine;
        
        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            _buildSequenceStateMachine.Perform(context);
        }
        
        public void OnCastInput(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.PreviousState();
        }
        
        public void OnInteractionInput(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.NextState();
        }

        public void OnInterruptCast(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.Cancel();
        }

        //TODO: how to destroy a building -> there must be an entry und exit script on a building, that can be addressed!
        //TODO: onPlacementComplete -> apply on position, only if valid

        public override bool TryCast(GameObject caster)
        {
            //initialize values and check validity
            var casterPosition = TileHelper.TransformPositionToInt2(caster.transform);
            var dropKernel = tileManager.GetTileKernelAt(casterPosition, kernelSize);
            
            if (!ScriptableObjectByType.TryGetByType(out List<BuildingRecipe> buildingRecipes)) return false;
            var validBuildings = ParseValidBuildings(caster, dropKernel, buildingRecipes);
            if (validBuildings.Count == 0) return false;
            
            //execute
            validBuildings[0].InstantiatedBuilding.SetActive(true);
            SetBuildingFocus();
            InitializeBuildSequenceStateMachine(dropKernel, validBuildings);
            return true;
        }

        private List<BuildData> ParseValidBuildings(GameObject caster, Tile[,] dropKernel, List<BuildingRecipe> buildingRecipes)
        {
            var validBuildings = new List<BuildData>();
            
            foreach (var buildingRecipe in buildingRecipes)
            {
                if (IsRecipeFit(CreateKernelItemCountPairs(dropKernel), buildingRecipe) &&
                    kernelSize >= buildingRecipe.requiredBuildingKernelSize)
                {
                    var instantiatedObject = Instantiate(buildingRecipe.building, caster.transform.position,
                        Quaternion.identity);
                    
                    instantiatedObject.SetActive(false);
                    validBuildings.Add(new BuildData(instantiatedObject, buildingRecipe.recipeData.ToList()));
                }
            }

            return validBuildings;
        }
        
        private void SetBuildingFocus()
        {
            directionInputFocus.SetFocus(this);
            interactionInputFocus.SetFocus(this);
            castInputFocus.SetFocus(this);
        }

        private void RestoreBuildingFocus()
        {
            directionInputFocus.Restore();
            interactionInputFocus.Restore();
            castInputFocus.Restore();
        }

        private void InitializeBuildSequenceStateMachine(Tile[,] dropKernel, List<BuildData> validBuildings)
        {
            var onSequenceComplete = new Action<BuildData>(selectedBuilding =>
            {
                foreach (var tile in selectedBuilding.ObjectsToDestroy)
                {
                    tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(tile));
                }

                RestoreBuildingFocus();
                
                validBuildings.Remove(validBuildings.Find(x => x.InstantiatedBuilding == selectedBuilding.InstantiatedBuilding));
                DestroyAllBuildings(validBuildings);
                
                foreach (var baseTileRegistrator in selectedBuilding.InstantiatedBuilding.GetComponentsInChildren<BaseTileRegistrator>())
                {
                    baseTileRegistrator.RegisterOnTile();
                }
            });

            var onCancelSequence = new Action(() =>
            {
                RestoreBuildingFocus();
                DestroyAllBuildings(validBuildings);
            });
            
            IBuildSequenceState entrySequenceState;
            if (validBuildings.Count == 1)
            {
                entrySequenceState = new BuildingPlacementSequenceState(tileManager, validBuildings, 0, dropKernel);
            }
            else
            {
                entrySequenceState = new BuildingSelectionSequenceState(tileManager, validBuildings, dropKernel);
                
            }
            _buildSequenceStateMachine = new BuildSequenceStateMachine(entrySequenceState, onSequenceComplete, onCancelSequence);
        }

        private void DestroyAllBuildings(List<BuildData> validBuildings)
        {
            for (var i = validBuildings.Count - 1; i >= 0; i--)
            {
                Destroy(validBuildings[i].InstantiatedBuilding);
            }
        }

        private Dictionary<BaseItem_SO, int> CreateKernelItemCountPairs(Tile[,] tileKernel)
        {
            var itemCountPairs = new Dictionary<BaseItem_SO, int>();

            foreach (var tile in tileKernel)
            {
                if (!tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) continue;
                
                if (!itemCountPairs.ContainsKey(tile.ItemContainer.ContainedBaseItem))
                {
                    itemCountPairs.Add(tile.ItemContainer.ContainedBaseItem, 1);
                }
                else
                {
                    itemCountPairs[tile.ItemContainer.ContainedBaseItem]++;
                }
            }
            
            return itemCountPairs;
        }
        
        private bool IsRecipeFit(Dictionary<BaseItem_SO, int> itemCountPairs, BuildingRecipe buildingRecipe)
        {
            foreach (var recipeData in buildingRecipe.recipeData)
            {
                if (!itemCountPairs.TryGetValue(recipeData.requiredItem, out int count) ||
                    recipeData.requiredCount > count)
                {
                    return false;
                }
            }
            
            return true;
        }
    }

    public class BuildData
    {
        public readonly GameObject InstantiatedBuilding;
        public readonly List<RecipeData> RecipeData;
        public readonly List<Tile> ObjectsToDestroy;

        public BuildData(GameObject instantiatedBuilding, List<RecipeData> recipeData)
        {
            ObjectsToDestroy = new List<Tile>();
            InstantiatedBuilding = instantiatedBuilding;
            RecipeData = recipeData;
        }
    }
}
