using System.Collections.Generic;
using System.Linq;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingRotationSequenceState : IBuildSequenceState
{
    private readonly TileManager _tileManager;
    private readonly List<GameObject> _validBuildings;
    private readonly int _selectedIndex;
    private readonly Tile[,] _buildArea;

    public BuildingRotationSequenceState(TileManager tileManager, List<GameObject> validBuildings, int selectedIndex, Tile[,] buildArea)
    {
        _tileManager = tileManager;
        _validBuildings = validBuildings;
        _selectedIndex = selectedIndex;
        _buildArea = buildArea;
    }

    public bool TryGetNext(out IBuildSequenceState nextState)
    {
        nextState = default;
        if (!BuildingPlacementIsValid(_validBuildings[_selectedIndex])) return false;
        
        nextState = this;
        return true;
    }

    public bool TryGetPrevious(out IBuildSequenceState nextState)
    {
        nextState = new BuildingPlacementSequenceState(_tileManager, _validBuildings, _selectedIndex, _buildArea);
        return true;
    }

    public void OnPerform(InputAction.CallbackContext context)
    {
        //TODO: rotation orientates itself towards world position zero
        
        var inputVector = context.ReadValue<Vector2>();

        var interactables = _validBuildings[_selectedIndex].GetComponentsInChildren<BaseTileRegistrator>();

        var position = new int2();
        position = interactables.Aggregate(position, (current, interactable) => current + _tileManager.GetTileAt(TileHelper.TransformPositionToInt2(interactable.transform))
            .WorldPosition);
        position /= interactables.Length;

        if (inputVector == Vector2.left)
        {
            Rotate(-90, new Vector3(position.x, 0, position.y));
        }
        else if (inputVector == Vector2.right)
        {
            Rotate(90, new Vector3(position.x, 0, position.y));
        }
        
        if (BuildingPlacementIsValid(_validBuildings[_selectedIndex]))
        {
            Debug.Log("CanBuild");
        }
    }
    
    private void Rotate(float angle, Vector3 rotateAround)
    {
        _validBuildings[_selectedIndex].transform.RotateAround(rotateAround, Vector3.up, angle);
    }
    
    private bool BuildingPlacementIsValid(GameObject building)
    {
        var buildingObjects = building.GetComponentsInChildren<BaseTileRegistrator>();

        return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
    }
}
