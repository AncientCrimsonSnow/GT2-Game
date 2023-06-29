using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildVisualization : MonoBehaviour
{
    [SerializeField] private SpriteRenderer selection;
    [SerializeField] private SpriteRenderer movement;
    [SerializeField] private SpriteRenderer rotation;

    private void Awake()
    {
        DisableAll();
    }

    public void SetAllColor(bool placeable)
    {
        Color color = placeable ? Color.green : Color.red;

        selection.color = color;
        movement.color = color;
        rotation.color = color;
    }

    public void DisableAll()
    {
        selection.gameObject.SetActive(false);
        movement.gameObject.SetActive(false);
        rotation.gameObject.SetActive(false);
    }
    
    public void EnableSelection()
    {
        selection.gameObject.SetActive(true);
        movement.gameObject.SetActive(false);
        rotation.gameObject.SetActive(false);
    }
    
    public void EnableMovement()
    {
        selection.gameObject.SetActive(false);
        movement.gameObject.SetActive(true);
        rotation.gameObject.SetActive(false);
    }
    
    public void EnableRotation()
    {
        selection.gameObject.SetActive(false);
        movement.gameObject.SetActive(false);
        rotation.gameObject.SetActive(true);
    }
}
