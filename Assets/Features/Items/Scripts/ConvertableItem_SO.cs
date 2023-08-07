using System.Collections;
using System.Collections.Generic;
using Features.Items.Scripts;
using Features.TileSystem.Scripts;
using Uilities.Pool;
using UnityEngine;

[CreateAssetMenu]
public class ConvertableItem_SO : BaseItem_SO
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private BaseItem_SO requiredCarriedItem;
    [SerializeField] private BaseItem_SO newItem;

    public override bool TryCast(GameObject caster)
    {
        if (!caster.TryGetComponent(out IItemCarryBehaviour itemCarryBehaviour)) return false;
        if (!itemCarryBehaviour.IsCarrying()) return false;
        if (itemCarryBehaviour.GetNextCarried() != requiredCarriedItem) return false;
        
        itemCarryBehaviour.DropItem(requiredCarriedItem);

        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(caster.transform));
        
        tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(tile));

        var tileInteractable = new UnstackableItemTileInteractable(tile, true, newItem,
            newItem.prefab.Reuse(caster.transform.position, Quaternion.identity));
        tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(tileInteractable);
        
        return true;
    }
}
