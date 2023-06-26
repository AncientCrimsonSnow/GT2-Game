using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Buildings.Scripts
{
    public class ScriptableObjectByType : ScriptableObject, ISerializationCallbackReceiver
    {
        private static readonly Dictionary<Type, List<ScriptableObjectByType>> Entries =
            new Dictionary<Type, List<ScriptableObjectByType>>();

        public static bool TryGetByType<T>(out List<T> output) where T : ScriptableObjectByType
        {
            output = default;
        
            if (!Entries.TryGetValue(typeof(T), out List<ScriptableObjectByType> scriptableObjectByTypes)) return false;

            output = scriptableObjectByTypes.ConvertAll(x => x as T);
            return true;
        }
        
        protected virtual void OnEnable()
        {
            RegisterObject(this);
        }

        protected virtual void OnDestroy()
        {
            UnregisterObject(this);
        }

        public virtual void OnAfterDeserialize()
        {
            RegisterObject(this);
        }

        public virtual void OnBeforeSerialize()
        {
            RegisterObject(this);
        }

        private static void RegisterObject(ScriptableObjectByType obj)
        {
            if (Entries.TryGetValue(obj.GetType(), out var existingList))
            {
                if (!existingList.Contains(obj))
                {
                    existingList.Add(obj);
                }
            }
            else
            {
                Entries.Add(obj.GetType(), new List<ScriptableObjectByType>{obj});
            }
        }

        private static void UnregisterObject(ScriptableObjectByType obj)
        {
            if (!Entries.TryGetValue(obj.GetType(), out var existingList)) return;
        
            if (existingList.Contains(obj))
            {
                existingList.Remove(obj);
            }
        }
    }
}
