using System;
using UnityEngine;

namespace Uilities.Event
{
    public abstract class ActionEventWithParameter<T> : ScriptableObject
    {
        private Action<T> listeners;
    
        public void Raise(T parameter)
        {
            this.listeners?.Invoke(parameter);
        }

        public void RegisterListener(Action<T> listener)
        {
            this.listeners += listener;
        }

        public void UnregisterListener(Action<T> listener)
        {
            this.listeners -= listener;
        }
    }
}