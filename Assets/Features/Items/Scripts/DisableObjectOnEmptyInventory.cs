using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

public class DisableObjectOnEmptyInventory : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject gameObjectToHide;
    
    void Update()
    {
        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        gameObjectToHide.SetActive(tile.ItemContainer.ItemCount != 0);
    }
}
