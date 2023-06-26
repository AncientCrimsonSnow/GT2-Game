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
    
    public bool TryCompleteSequence(out IBuildSequenceState nextState)
    {
        nextState = new BuildingRotationSequenceState(_tileManager, _validBuildings, _selectedIndex, _buildArea);
        return false;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = new BuildingSelectionSequenceState(_tileManager, _validBuildings, _buildArea);
        return true;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
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

    public GameObject GetSelectedObject()
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
