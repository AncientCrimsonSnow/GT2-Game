using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReversePositionRecord : IRecord
    {
        private readonly Transform _transform;
        private readonly Vector3 _position;
        private readonly bool _forwardReplay;

        public ReversePositionRecord(Transform transform, Vector3 position)
        {
            _transform = transform;
            _position = position;
        }

        public void PerformRecordBehaviourForward() { }

        public void PerformRecordBehaviourReverse()
        {
            _transform.position = _position;
        }
    }
}