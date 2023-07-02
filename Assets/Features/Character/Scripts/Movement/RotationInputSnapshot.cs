using Features.ReplaySystem.Record;
using UnityEngine;

namespace Features.Character.Scripts.Movement
{
    public class RotationInputSnapshot : IInputSnapshot
    {
        private readonly Transform _transform;
        private readonly Vector2 _storedInputVector;

        public RotationInputSnapshot(Transform transform, Vector2 storedInputVector)
        {
            _transform = transform;
            _storedInputVector = storedInputVector;
        }
    
        public void Tick(float tickDurationInSeconds)
        {
            _transform.rotation = Quaternion.LookRotation(new Vector3(_storedInputVector.x, _transform.position.y, _storedInputVector.y));
        }
    }
}
