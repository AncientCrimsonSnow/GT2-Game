using System;
using System.Collections;
using System.Collections.Generic;
using NewReplaySystem;
using UnityEngine;

namespace NewReplaySystem
{
    public class ForwardPositionRecord : IRecord/*, IEquatable<ForwardPositionRecord>*/
    {
        private readonly Transform _transform;
        private readonly Vector3 _position;

        public ForwardPositionRecord(Transform transform, Vector3 position)
        {
            _transform = transform;
            _position = position;
        }

        public void PerformRecordBehaviourForward()
        {
            _transform.position = _position;
        }

        public void PerformRecordBehaviourReverse() { }

        /*
        public bool Equals(ForwardPositionRecord other)
        {
            return Equals(_transform, other._transform) && _position.Equals(other._position);
        }

        public override bool Equals(object obj)
        {
            return obj is ForwardPositionRecord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_transform, _position);
        }*/
    }
}
