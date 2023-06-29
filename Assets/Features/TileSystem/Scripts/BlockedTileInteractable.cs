using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.Scripts;
using UnityEngine;

public class BlockedTileInteractable : ITileInteractable
{
    public bool TryInteract(GameObject interactor)
    {
        return false;
    }

    public bool TryCast(GameObject caster)
    {
        return false;
    }

    public bool IsMovable()
    {
        return false;
    }
}
