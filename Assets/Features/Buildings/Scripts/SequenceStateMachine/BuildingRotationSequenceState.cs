using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Buildings.Scripts.SequenceStateMachine
{
    public class BuildingRotationSequenceState : IBuildSequenceState
    {
        private readonly TileManager _tileManager;
        private readonly List<BuildData> _validBuildings;
        private readonly int _selectedIndex;
        private readonly Tile[,] _buildArea;

        public BuildingRotationSequenceState(TileManager tileManager, List<BuildData> validBuildings, int selectedIndex, Tile[,] buildArea)
        {
            _tileManager = tileManager;
            _validBuildings = validBuildings;
            _selectedIndex = selectedIndex;
            _buildArea = buildArea;
        
            var buildVisualization = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentInChildren<BuildVisualization>();
            if (buildVisualization != null)
            {
                buildVisualization.EnableRotation();
                buildVisualization.SetAllColor(BuildingPlacementIsValid(_validBuildings[_selectedIndex].InstantiatedBuilding));
            }
        }

        public bool TryCompleteSequence(out IBuildSequenceState nextState)
        {
            nextState = default;
            if (BuildingPlacementIsValid(_validBuildings[_selectedIndex].InstantiatedBuilding))
            {
                var buildVisualization = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentInChildren<BuildVisualization>();
                if (buildVisualization != null)
                {
                    buildVisualization.DisableAll();
                }
            
                return true;
            }
        
            nextState = this;
            return false;
        }

        public bool TryGetPrevious(out IBuildSequenceState nextState)
        {
            nextState = new BuildingPlacementSequenceState(_tileManager, _validBuildings, _selectedIndex, _buildArea);
            return true;
        }

        public void OnPerform(InputAction.CallbackContext context)
        {
            var inputVector = context.ReadValue<Vector2>();

            var interactables = _validBuildings[_selectedIndex].InstantiatedBuilding.GetComponentsInChildren<BaseTileRegistrator>();

            var position = new int2();
            position = interactables.Aggregate(position, (current, interactable) => current + _tileManager.GetTileAt(TileHelper.TransformPositionToInt2(interactable.transform))
                .WorldPosition);
            var newPosition = new Vector3(position.x / (float)interactables.Length, 0, position.y / (float)interactables.Length);

            if (inputVector == Vector2.left)
            {
                Rotate(-90, newPosition);
            }
            else if (inputVector == Vector2.right)
            {
                Rotate(90, newPosition);
            }
        
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

        private void Rotate(float angle, Vector3 rotateAround)
        {
            _validBuildings[_selectedIndex].InstantiatedBuilding.transform.RotateAround(rotateAround, Vector3.up, angle);
        }
    
        private bool BuildingPlacementIsValid(GameObject building)
        {
            var buildingObjects = building.GetComponentsInChildren<BaseTileRegistrator>();

            return buildingObjects.All(baseTileInteractableRegistrator => baseTileInteractableRegistrator.CanRegisterOnTile());
        }
    }
}
