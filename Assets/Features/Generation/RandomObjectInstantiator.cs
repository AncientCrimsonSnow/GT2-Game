using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using JetBrains.Annotations;
using UnityEngine;

public class RandomObjectInstantiator : MonoBehaviour
{
    [SerializeField] private Vector2Int rangeY;
    [SerializeField] private Vector2Int rangeX;
    [SerializeField] private GameObject objectToInstantiate;
    [SerializeField] private Transform parent;
    [SerializeField] private int count;

    [Button, UsedImplicitly]
    public void Generate()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(objectToInstantiate,
                new Vector3(Random.Range(rangeX.x, rangeX.y), 0, Random.Range(rangeY.x, rangeY.y)),
                Quaternion.identity, parent);
        }
    }
    
    [Button, UsedImplicitly]
    public void DestroyChilds()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            else
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
