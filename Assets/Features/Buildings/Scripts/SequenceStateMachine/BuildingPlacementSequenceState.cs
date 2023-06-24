using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacementSequenceState : IBuildSequenceState
{
    private readonly TileManager _tileManager;
    private readonly List<GameObject> _validBuildings;
    private readonly int _selectedIndex;
    private readonly Tile[,] _buildArea;

    public BuildingPlacementSequenceState(TileManager tileManager, List<GameObject> validBuildings, int selectedIndex, Tile[,] buildArea)
    {
        _tileManager = tileManager;
        _validBuildings = validBuildings;
        _selectedIndex = selectedIndex;
        _buildArea = buildArea;
    }
    
    public bool TryGetNext(out IBuildSequenceState nextState)
    {
        nextState = new BuildingRotationSequenceState(_tileManager, _validBuildings, _selectedIndex, _buildArea);
        return true;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = new BuildingSelectionSequenceState(_tileManager, _validBuildings, _buildArea);
        return true;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        //TODO: not quite right yet
        
        var inputVector = context.ReadValue<Vector2>();
        var movementInt2 = new int2((int)inputVector.x, (int)inputVector.y);

        var interactables = _validBuildings[_selectedIndex].GetComponentsInChildren<BaseTileRegistrator>();
        var buildingPositions = interactables
            .Select(interactable => _tileManager.GetTileAt(TileHelper.TransformPositionToInt2(interactable.transform)).ArrayPosition)
            .ToList();

        if (!CanBeMoved(_buildArea, buildingPositions, movementInt2)) return;
        
        _validBuildings[_selectedIndex].transform.position += new Vector3(inputVector.x, 0, inputVector.y);
        if (BuildingPlacementIsValid(_validBuildings[_selectedIndex]))
        {
            Debug.Log("CanBuild");
        }
    }

    private bool CanBeMoved(Tile[,] buildingKernel, List<int2> buildArea, int2 movement)
    {
        var lowerBoundaryInclusive = buildingKernel[0, 0].ArrayPosition;
        var higherXBoundaryExclusive = buildingKernel[buildingKernel.GetLength(0) - 1, 0].ArrayPosition.x;
        var higherYBoundaryExclusive = buildingKernel[0, buildingKernel.GetLength(1) - 1].ArrayPosition.x;

        return buildArea
            .Select(tileArrayPosition => tileArrayPosition + movement)
            .All(newPos => newPos.x >= lowerBoundaryInclusive.x && newPos.x < higherXBoundaryExclusive && 
                           newPos.y >= lowerBoundaryInclusive.y && newPos.y < higherYBoundaryExclusive);
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
    }
}
