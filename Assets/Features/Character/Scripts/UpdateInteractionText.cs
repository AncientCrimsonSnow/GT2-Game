using System.Collections;
using System.Collections.Generic;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Magic;
using Features.Items.Scripts;
using Features.TileSystem.Scripts;
using TMPro;
using UnityEngine;

public class UpdateInteractionText : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TMP_Text interaction;
    [SerializeField] private TMP_Text cast;

    private GameObject _caster;

    public void SetCaster(GameObject caster)
    {
        _caster = caster;
    }
    
    void Update()
    {
        transform.localPosition = Vector3.zero;
        var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));

        if (tile.CanInteract(_caster, out string interactionText) && _caster.GetComponent<InteractionInputBehaviour>() != null && !BuildReplayMagicItem_SO.IsBuilding)
        {
            interaction.text = interactionText;
            interaction.gameObject.SetActive(true);
        }
        else
        {
            interaction.gameObject.SetActive(false);
        }
        
        if (tile.CanCast(_caster, out string castText) && !BuildReplayMagicItem_SO.IsBuilding)
        {
            cast.text = castText;
            cast.gameObject.SetActive(true);
        }
        else
        {
            cast.gameObject.SetActive(false);
        }
    }
}
