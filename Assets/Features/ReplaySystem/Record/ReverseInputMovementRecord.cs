using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReverseInputMovementRecord : IRecord
    {
        private readonly GameObject _characterController;
        private readonly Vector2 _inputMovement;
        private readonly bool _forwardReplay;


        public ReverseInputMovementRecord(GameObject characterController, Vector2 inputMovement)
        {
            _characterController = characterController;
            _inputMovement = inputMovement;
        }

        public void PerformRecordBehaviourForward() { }

        public void PerformRecordBehaviourReverse()
        {
            //_characterController.SetMovementAnimation(_inputMovement);
        }
    }
}
