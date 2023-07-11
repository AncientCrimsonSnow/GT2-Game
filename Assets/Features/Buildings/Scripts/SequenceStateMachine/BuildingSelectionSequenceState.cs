using System.Collections.Generic;
using Features.Items.Scripts;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Buildings.Scripts.SequenceStateMachine
{
    public class BuildingSelectionSequenceState : IBuildSequenceState
    {
        private readonly TileManager _tileManager;
        private readonly List<BuildData> _validBuildings;
        private readonly Tile[,] _buildArea;
        private int _currentIndex;

        public BuildingSelectionSequenceState(TileManager tileManager, List<BuildData> validBuildings, Tile[,] buildArea, int selectedIndex = 0)
        {
            _tileManager = tileManager;
            _validBuildings = validBuildings;
            _buildArea = buildArea;
            _currentIndex = selectedIndex;

            _validBuildings[_currentIndex].ResetColor(visualization => visualization.EnableSelection());
        } 
    
        public bool TryCompleteSequence(out IBuildSequenceState nextState)
        {
            _validBuildings[_currentIndex].ReleaseBuildingAreaObjects();
            nextState = new BuildingPlacementSequenceState(_tileManager, _validBuildings, _currentIndex, _buildArea);
            return false;
        }

        public bool TryGetPrevious(out IBuildSequenceState nextState)
        {
            nextState = default;
            return false;
        }

        public void OnPerform(InputAction.CallbackContext context)
        {
            var inputVector = context.ReadValue<Vector2>();

            switch (inputVector.x)
            {
                case < 0:
                    SetNewIndex(-1);
                    break;
                case > 0:
                    SetNewIndex(1);
                    break;
            }
        }

        public BuildData GetSelectedObject()
        {
            return _validBuildings[_currentIndex];
        }

        private void SetNewIndex(int addition)
        {
            _validBuildings[_currentIndex].InstantiatedBuilding.gameObject.SetActive(false);
            
            _currentIndex += addition;

            if (_currentIndex >= _validBuildings.Count)
            {
                _currentIndex = 0;
            }

            if (_currentIndex < 0)
            {
                _currentIndex = _validBuildings.Count - 1;
            }
            
            _validBuildings[_currentIndex].InstantiatedBuilding.gameObject.SetActive(true);
            
            _validBuildings[_currentIndex].ResetColor(visualization => visualization.EnableSelection());
        }
    }
}