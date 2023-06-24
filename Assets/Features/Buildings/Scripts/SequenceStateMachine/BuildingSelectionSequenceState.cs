
using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelectionSequenceState : IBuildSequenceState
{
    private readonly TileManager _tileManager;
    private readonly List<GameObject> _validBuildings;
    private readonly Tile[,] _buildArea;
    private int _currentIndex;

    public BuildingSelectionSequenceState(TileManager tileManager, List<GameObject> validBuildings, Tile[,] buildArea, int selectedIndex = 0)
    {
        _tileManager = tileManager;
        _validBuildings = validBuildings;
        _buildArea = buildArea;
        _currentIndex = selectedIndex;
    } 
    
    public bool TryGetNext(out IBuildSequenceState nextState)
    {
        nextState = new BuildingPlacementSequenceState(_tileManager, _validBuildings, _currentIndex, _buildArea);
        return true;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = default;
        return false;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();

        Debug.Log(inputVector.x);
        switch (inputVector.x)
        {
            case < 0:
                SetNewIndex(-1);
                break;
            case > 0:
                SetNewIndex(1);
                break;
        }

        if (BuildingPlacementIsValid(_validBuildings[_currentIndex]))
        {
            //Debug.Log("CanBuild");
        }
    }

    private void SetNewIndex(int addition)
    {
        _validBuildings[_currentIndex].SetActive(false);
            
        _currentIndex += addition;

        if (_currentIndex >= _validBuildings.Count)
        {
            _currentIndex = 0;
        }

        if (_currentIndex < 0)
        {
            _currentIndex = _validBuildings.Count - 1;
        }
            
        _validBuildings[_currentIndex].SetActive(true);
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
    }
}