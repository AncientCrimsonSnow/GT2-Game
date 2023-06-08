using UnityEngine;

namespace NewReplaySystem
{
    public class PositionRecord : IRecord
    {
        private readonly Transform _transform;
        private readonly Vector3 _position;
        private readonly bool _forwardReplay;

        public PositionRecord(Transform transform, Vector3 position)
        {
            _transform = transform;
            _position = position;
        }

        public void PerformRecordBehaviourForward()
        {
            _transform.position = _position;
        }

        public void PerformRecordBehaviourReverse()
        {
            _transform.position = _position;
        }
    }
}
