using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ForwardInputMovementRecord : IRecord
    {
        private readonly GameObject _characterController;
        private readonly Vector2 _inputMovement;


        public ForwardInputMovementRecord(GameObject characterController, Vector2 inputMovement)
        {
            _characterController = characterController;
            _inputMovement = inputMovement;
        }

        public void PerformRecordBehaviourForward()
        {
            //_characterController.SetMovementAnimation(_inputMovement);
        }

        public void PerformRecordBehaviourReverse() { }
    }
}
