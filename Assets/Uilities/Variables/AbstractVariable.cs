using DataStructures.Event;
using UnityEngine;

namespace DataStructures.Variables
{
    public abstract class AbstractVariable<T> : ScriptableObject
    {
        [SerializeField] protected T runtimeValue;
        [SerializeField] private T storedValue;
        [SerializeField] protected GameEvent onValueChanged;

        private void OnEnable()
        {
            Restore();
        }

        public void Restore()
        {
            runtimeValue = storedValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public T Get() => runtimeValue;

        public void Set(T value)
        {
            if (value.Equals(runtimeValue)) return;
            
            runtimeValue = value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Copy(AbstractVariable<T> other) => runtimeValue = other.runtimeValue;
    }
}
