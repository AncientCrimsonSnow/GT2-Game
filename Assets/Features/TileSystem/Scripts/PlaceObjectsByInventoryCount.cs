using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

public class PlaceObjectsByInventoryCount : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject oneObject;
    [SerializeField] private GameObject twoObjects;
    [SerializeField] private GameObject threeObjects;
    
    void Update()
    {
        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        oneObject.SetActive(tile.ItemContainer.ItemCount > 0);
        twoObjects.SetActive(tile.ItemContainer.ItemCount > 1);
        threeObjects.SetActive(tile.ItemContainer.ItemCount > 2);
    }
}
