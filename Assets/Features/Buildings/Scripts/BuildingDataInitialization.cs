using System.Collections.Generic;
using UnityEngine;

namespace Features.Buildings.Scripts
{
    public class BuildingDataInitialization : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObjectByType> scriptableObjectByType;

        private void Awake()
        {
            foreach (var objectByType in scriptableObjectByType)
            {
                ScriptableObjectByType.RegisterObject(objectByType);
            }
        }
    }
}
