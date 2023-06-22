using System.Collections.Generic;
using System.Linq;
using Features.Buildings.Scripts;
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

        
        private BuildSequenceStateMachine _buildSequenceStateMachine;
        
        public void OnDirectionInput(InputAction.CallbackContext context)
        {
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

        //TODO: use case: building
        //TODO: button e: next step | button q: go step back | button f: cancel building -> i need to set every single focus
        //TODO: step 1: select building with a/d | if there is only one building -> skip
        //TODO: step 2: position building inside kernel, if building smaller than kernel - use WASD | if building is 1x1 -> skip
        //TODO: step 3: rotate building inside kernel with a/d | if building is 1x1 -> skip
        //TODO: step 4: place building, if valid -> final check at the end -> after each change, visualisation will be updated, but user can go to next step
        //TODO: use state machine? create interface for every single input!
        
        //TODO: how to destroy a building -> there must be an entry und exit script on a building, that can be addressed!

        public override bool TryCast(GameObject caster)
        {
            directionInputFocus.SetFocus(this);
            var dropKernel = tileManager.GetTileKernelAt(TileHelper.TransformPositionToInt2(caster.transform), kernelSize);
            
            if (!ScriptableObjectByType.TryGetByType(out List<BuildingRecipe> buildingRecipes)) return false;

            var validBuildings = new List<GameObject>();
            foreach (var buildingRecipe in buildingRecipes)
            {
                if (IsRecipeFit(CreateKernelItemCountPairs(dropKernel), buildingRecipe))
                {
                    Debug.Log(buildingRecipe);
                    var instantiatedObject = Instantiate(buildingRecipe.building, caster.transform.position, Quaternion.identity);
                    instantiatedObject.SetActive(false);
                    validBuildings.Add(instantiatedObject);
                    //TODO: onPlacementPerformed -> event for destroying all items on those slots
                    //TODO: onBuildingDestroyed -> event for what happens, if building got destroyed
                }
            }

            //TODO: instantiated ghost is just there for validating, if the building can be placed. when a building got placed, it will be registered! Stacks must be registered as well, but they dont get instantiated!

            _buildSequenceStateMachine = new BuildSequenceStateMachine(new BuildingSelectionSequenceState(validBuildings), () => {}, () => {});
            
            return true;
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
}
