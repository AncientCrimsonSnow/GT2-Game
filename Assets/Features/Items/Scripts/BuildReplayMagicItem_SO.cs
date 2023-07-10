using System;
using System.Collections.Generic;
using System.Linq;
using Features.Buildings.Scripts;
using Features.Buildings.Scripts.SequenceStateMachine;
using Features.Character.Scripts;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Magic;
using Features.Character.Scripts.Movement;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Uilities.Pool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class BuildReplayMagicItem_SO : BaseItem_SO, IDirectionInput, ICastInput, IInteractInput
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private int kernelSize = 3;

        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private DirectionInputFocus directionInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private CastInputFocus castInputFocus;

        private BuildSequenceStateMachine _buildSequenceStateMachine;

        public void OnDirectionInputFocusChanges() { }

        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            _buildSequenceStateMachine.Perform(context);
        }
        
        public void OnCastInput(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.NextState();
        }
        
        public void OnInteractionInput(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.PreviousState();
        }

        public void OnInterruptCast(InputAction.CallbackContext context)
        {
            _buildSequenceStateMachine.Cancel();
        }

        public override bool TryCast(GameObject caster)
        {
            if (skeletonFocus.GetFocus() != caster) return false;
            
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
                    validBuildings.Add(new BuildData(tileManager, instantiatedObject, CopyData(buildingRecipe.recipeData), dropKernel));
                }
            }
            
            return validBuildings;
        }

        private List<RecipeData> CopyData(List<RecipeData> listToBeCopied)
        {
            var newList = new List<RecipeData>();

            foreach (var recipeData in listToBeCopied)
            {
                newList.Add(new RecipeData(recipeData.requiredItem, recipeData.requiredCount));
            }

            return newList;
        }
        
        private void SetBuildingFocus()
        {
            directionInputFocus.PushFocus(this);
            interactionInputFocus.PushFocus(this);
            castInputFocus.PushFocus(this);
        }

        private void RestoreBuildingFocus()
        {
            directionInputFocus.PopFocus();
            interactionInputFocus.PopFocus();
            castInputFocus.PopFocus();
        }

        private void InitializeBuildSequenceStateMachine(Tile[,] dropKernel, List<BuildData> validBuildings)
        {
            var onSequenceComplete = new Action<BuildData>(selectedBuilding =>
            {
                RestoreBuildingFocus();
                
                validBuildings.Remove(validBuildings.Find(x => x.InstantiatedBuilding == selectedBuilding.InstantiatedBuilding));
                DestroyAllBuildings(validBuildings);

                selectedBuilding.ApplyBuild();
                
                ReplayManager.Instance.StopReplayable(skeletonFocus.GetFocus(), true);
            });

            var onCancelSequence = new Action(() =>
            {
                RestoreBuildingFocus();
                DestroyAllBuildings(validBuildings);
            });
            
            IBuildSequenceState entrySequenceState;
            if (validBuildings.Count == 1)
            {
                validBuildings[0].ReleaseBuildingAreaObjects();
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
        
        private readonly TileManager _tileManager;
        private readonly Dictionary<Tile, Poolable> _neededPoolables;
        private readonly BuildVisualization _buildVisualization;

        public BuildData(TileManager tileManager, GameObject instantiatedBuilding, List<RecipeData> recipeDataList, Tile[,] buildArea)
        {
            _neededPoolables = new Dictionary<Tile, Poolable>();
            _tileManager = tileManager;
            InstantiatedBuilding = instantiatedBuilding;
            
            _buildVisualization = instantiatedBuilding.GetComponentInChildren<BuildVisualization>();
            
            foreach (var kernelTile in buildArea)
            {
                if (!kernelTile.ItemContainer.ContainsItem() || !kernelTile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) continue;

                foreach (var recipeData in recipeDataList)
                {
                    if (kernelTile.ItemContainer.ContainedBaseItem == recipeData.requiredItem && recipeData.requiredCount > 0)
                    {
                        _neededPoolables.Add(kernelTile, kernelTile.ItemContainer.PooledGameObject);
                        recipeData.ReduceRequiredCount();
                        break;
                    }
                }
            }
        }

        public void ApplyColor(Action<BuildVisualization> onVisualize = null)
        {
            if (_buildVisualization != null)
            {
                onVisualize?.Invoke(_buildVisualization);
                _buildVisualization.SetAllColor(BuildingPlacementIsValid());
            }
        }
        
        private bool BuildingPlacementIsValid()
        {
            var buildingObjects = InstantiatedBuilding.GetComponentsInChildren<BaseTileRegistrator>();

            return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
        }
        
        public void ReuseBuildingAreaObjects()
        {
            List<BaseItem_SO> objectsToReplace = new List<BaseItem_SO>();

            foreach (var keyValuePair in _neededPoolables)
            {
                if (keyValuePair.Key.ItemContainer.ContainsItem())
                {
                    objectsToReplace.Add(keyValuePair.Key.ItemContainer.ContainedBaseItem);
                    keyValuePair.Key.ItemContainer.PooledGameObject.Release();
                }

                var position = new Vector3(keyValuePair.Key.WorldPosition.x, 0, keyValuePair.Key.WorldPosition.y);
                keyValuePair.Value.Reuse(position, Quaternion.identity);
            }
            
            foreach (var baseItemSo in objectsToReplace)
            {
                TileHelper.DropItemNearestEmptyTile(_tileManager, InstantiatedBuilding.transform.position, baseItemSo);
            }
        }

        public void ReleaseBuildingAreaObjects()
        {
            foreach (var keyValuePair in _neededPoolables)
            {
                keyValuePair.Value.Release();
            }
        }

        public bool CanApplyBuild()
        {
            if (!BuildingPlacementIsValid()) return false;
            
            if (_buildVisualization != null)
            {
                _buildVisualization.DisableAll();
            }
            
            return true;
        }

        public void ApplyBuild()
        {
            foreach (var baseTileRegistrator in InstantiatedBuilding.GetComponentsInChildren<BaseTileRegistrator>())
            {
                baseTileRegistrator.RegisterOnTile();
            }
        }
    }
}
