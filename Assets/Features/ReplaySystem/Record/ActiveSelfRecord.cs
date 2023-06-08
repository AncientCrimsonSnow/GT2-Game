using UnityEngine;

namespace NewReplaySystem
{
    public class ActiveSelfRecord : IRecord
    {
        private readonly GameObject _gameObject;
        private readonly bool _isActive;

        public ActiveSelfRecord(GameObject gameObject, bool isActive)
        {
            _gameObject = gameObject;
            _isActive = isActive;
        }

        public void PerformRecordBehaviourForward()
        {
            _gameObject.SetActive(_isActive);
        }

        public void PerformRecordBehaviourReverse()
        {
            _gameObject.SetActive(!_isActive);
        }
    }
}
