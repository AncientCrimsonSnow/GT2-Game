using System.Collections.Generic;
using Features.Buildings.Scripts;
using UnityEngine;

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
