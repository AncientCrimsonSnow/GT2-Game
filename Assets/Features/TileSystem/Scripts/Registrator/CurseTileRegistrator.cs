using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.Scripts.Registrator;
using Uilities.Variables;
using UnityEngine;

public class CurseTileRegistrator : BaseTileRegistrator
{
    [SerializeField] private FloatVariable currentMagicValue;
    [SerializeField] private GameObject curseVFX;
    [SerializeField] private float neededMagicValue;
    [SerializeField] private float time;

    private CurseInteractable curseInteractable;

    public override bool CanRegisterOnTile()
    {
        return true;
    }

    protected override void InternalRegisterOnTile()
    {
        curseInteractable = new CurseInteractable(currentMagicValue, curseVFX, neededMagicValue, this, time);
        Tile.RegisterTileInteractable(curseInteractable);
    }

    protected override void InternalUnregisterOnTile()
    {
        if (curseInteractable != null)
        {
            Tile.UnregisterTileInteractable(curseInteractable);
        }
    }
}
