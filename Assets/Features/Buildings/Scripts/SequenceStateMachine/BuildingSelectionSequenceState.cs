
using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.Scripts.Registrator;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelectionSequenceState : IBuildSequenceState
{
    private readonly List<GameObject> _validBuildings;
    private int _currentIndex;

    public BuildingSelectionSequenceState(List<GameObject> validBuildings)
    {
        _validBuildings = validBuildings;
    } 
    
    public bool TryGetNext(out IBuildSequenceState nextState)
    {
        nextState = new BuildingPlacementSequenceState(_validBuildings);
        return true;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = default;
        return false;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        _validBuildings[_currentIndex].SetActive(false);
            
        _currentIndex++;

        if (_currentIndex >= _validBuildings.Count)
        {
            _currentIndex = 0;
        }
            
        _validBuildings[_currentIndex].SetActive(true);
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileInteractableRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterTileInteractable());
    }
}