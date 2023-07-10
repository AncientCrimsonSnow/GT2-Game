using System;
using UnityEngine;

namespace DataStructures.Events
{
    public abstract class ParametrizedGameEvent<T> : ScriptableObject
    {
        private Action<T> listeners;
        
        public void Raise(T parameter)
        {
            listeners.Invoke(parameter);
        }

        public void RegisterListener(Action<T> listener)
        {
            listeners += listener;
        }

        public void UnregisterListener(Action<T> listener)
        {
            listeners -= listener;
        }
    }
}