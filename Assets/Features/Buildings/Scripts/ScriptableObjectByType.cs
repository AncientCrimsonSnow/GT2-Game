using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Features.Buildings.Scripts
{
    public class ScriptableObjectByType : ScriptableObject
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

        private void Awake()
        {
            RegisterObject(this);
        }

        private void OnEnable()
        {
            RegisterObject(this);
        }

        private void OnDestroy()
        {
            UnregisterObject(this);
        }

        public static void RegisterObject(ScriptableObjectByType obj)
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

        public static void UnregisterObject(ScriptableObjectByType obj)
        {
            if (!Entries.TryGetValue(obj.GetType(), out var existingList)) return;
        
            if (existingList.Contains(obj))
            {
                existingList.Remove(obj);
            }
        }
    }
}
