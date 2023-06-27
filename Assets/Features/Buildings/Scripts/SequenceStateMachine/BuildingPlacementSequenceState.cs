using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacementSequenceState : IBuildSequenceState
{
    private readonly TileManager _tileManager;
    private readonly List<BuildData> _validBuildings;
    private readonly int _selectedIndex;
    private readonly Tile[,] _buildArea;

    public BuildingPlacementSequenceState(TileManager tileManager, List<BuildData> validBuildings, int selectedIndex, Tile[,] buildArea)
    {
        _tileManager = tileManager;
        _validBuildings = validBuildings;
        _selectedIndex = selectedIndex;
        _buildArea = buildArea;
        
        foreach (var kernelTile in buildArea)
        {
            if (!kernelTile.ItemContainer.ContainsItem() || !kernelTile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>()) continue;
                    
            var foundRecipeData = validBuildings[selectedIndex].RecipeData.Find(x =>
                x.requiredItem == kernelTile.ItemContainer.ContainedBaseItem);

            if (foundRecipeData is not { requiredCount: > 0 }) continue;
            
            validBuildings[selectedIndex].ObjectsToDestroy.Add(kernelTile);
            kernelTile.ItemContainer.SetActive(false);
            foundRecipeData.requiredCount--;
        }

        var buildVisualization = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentInChildren<BuildVisualization>();
        if (buildVisualization != null)
        {
            buildVisualization.EnableMovement();
            buildVisualization.SetAllColor(BuildingPlacementIsValid(_validBuildings[_selectedIndex].InstantiatedBuilding));
        }
    }
    
    public bool TryCompleteSequence(out IBuildSequenceState nextState)
    {
        nextState = new BuildingRotationSequenceState(_tileManager, _validBuildings, _selectedIndex, _buildArea);
        return false;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        foreach (var tile in _validBuildings[_selectedIndex].ObjectsToDestroy)
        {
            tile.ItemContainer.SetActive(true);
        }
        _validBuildings[_selectedIndex].ObjectsToDestroy.Clear();
        
        nextState = new BuildingSelectionSequenceState(_tileManager, _validBuildings, _buildArea);
        return true;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();
        var movementInt2 = new int2((int)inputVector.x, (int)inputVector.y);

        var interactables = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentsInChildren<BaseTileRegistrator>();
        var buildingPositions = interactables
            .Select(interactable => _tileManager.GetTileAt(TileHelper.TransformPositionToInt2(interactable.transform)).ArrayPosition)
            .ToList();

        if (!CanBeMoved(_buildArea, buildingPositions, movementInt2)) return;
        
        _validBuildings[_selectedIndex].InstantiatedBuilding.transform.position += new Vector3(inputVector.x, 0, inputVector.y);
        
        var buildVisualization = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentInChildren<BuildVisualization>();
        if (buildVisualization != null)
        {
            buildVisualization.SetAllColor(BuildingPlacementIsValid(_validBuildings[_selectedIndex].InstantiatedBuilding));
        }
    }

    public BuildData GetSelectedObject()
    {
        return _validBuildings[_selectedIndex];
    }

    private bool CanBeMoved(Tile[,] buildArea, List<int2> buildingKernel, int2 movement)
    {
        var lowerBoundary = buildArea[0, 0].ArrayPosition;
        var higherXBoundary = buildArea[buildArea.GetLength(0) - 1, 0].ArrayPosition.x;
        var higherYBoundary = buildArea[0, buildArea.GetLength(1) - 1].ArrayPosition.y;

        return buildingKernel
            .Select(tileArrayPosition => tileArrayPosition + movement)
            .All(newPos => newPos.x >= lowerBoundary.x && newPos.x <= higherXBoundary && 
                           newPos.y >= lowerBoundary.y && newPos.y <= higherYBoundary);
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
    }
}