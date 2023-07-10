using System;
using UnityEngine;

namespace DataStructures.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Data/Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private Action listeners;
        
        public void Raise()
        {
            listeners?.Invoke();
        }

        public void RegisterListener(Action listener)
        {
            listeners += listener;
        }

        public void UnregisterListener(Action listener)
        {
            listeners -= listener;
        }
    }
}