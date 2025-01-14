using System;
using UnityEngine;

namespace Uilities.Event
{
    [CreateAssetMenu(fileName = "new ActionEvent", menuName = "DataStructures/Action Event")]
    public class ActionEvent : ScriptableObject
    {
        private Action listeners;
    
        public void Raise()
        {
            this.listeners?.Invoke();
        }

        public void RegisterListener(Action listener)
        {
            this.listeners += listener;
        }

        public void UnregisterListener(Action listener)
        {
            this.listeners -= listener;
        }
    }
}