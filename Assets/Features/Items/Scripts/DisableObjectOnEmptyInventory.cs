using System;
using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

public class DisableObjectOnEmptyInventory : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject gameObjectToHide;

    private void OnEnable()
    {
        CurseInteractable.OnCurseTriggered += Refill;
    }

    private void OnDisable()
    {
        CurseInteractable.OnCurseTriggered -= Refill;
    }

    private void Refill()
    {
        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        tile.ItemContainer.MaximizeItemCount();
    }

    void Update()
    {
        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));
        gameObjectToHide.SetActive(tile.ItemContainer.ItemCount != 0);
    }
}
