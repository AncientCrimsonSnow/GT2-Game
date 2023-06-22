using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.Scripts.Registrator;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacementSequenceState : IBuildSequenceState
{
    private readonly List<GameObject> _validBuildings;

    public BuildingPlacementSequenceState(List<GameObject> validBuildings)
    {
        _validBuildings = validBuildings;
    }
    
    public bool TryGetNext(out IBuildSequenceState nextState)
    {
        nextState = new BuildingRotationSequenceState(_validBuildings);
        return true;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = new BuildingSelectionSequenceState(_validBuildings);
        return true;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileInteractableRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterTileInteractable());
    }
}
