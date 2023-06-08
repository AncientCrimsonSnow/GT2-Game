using System.Collections;
using System.Collections.Generic;
using Features.TileSystem.CharacterBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Movement Input")]
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private BaseMovementInput entryMovementInput;
    
    [Header("Magic Input")]
    [SerializeField] private MagicInputFocus magicInputFocus;
    [SerializeField] private BaseMagicInput entryMagicInput;

    [Header("Interaction Input")]
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    
    [Header("Instantiaton")]
    [SerializeField] private GameObject magicInstantiationPrefab;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        movementInputFocus.SetFocus(entryMovementInput);
        magicInputFocus.SetFocus(entryMagicInput);
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInputFocus.OnMovementInput(context);
    }
    
    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        interactionInputFocus.OnInteractionInput(context);
    }
    
    public void OnMagicInput(InputAction.CallbackContext context)
    {
        //TODO: entry doesnt have interaction
        //TODO: if entry inputs: swap to new movement + interaction
        //TODO: if not, swaps to entry
        
        GameObject instantiated = Instantiate(magicInstantiationPrefab);
        instantiated.transform.position = movementInputFocus.GetFocus().transform.position;

        if (instantiated.TryGetComponent(out BaseMovementInput baseMovementInput))
        {
            movementInputFocus.SetFocus(baseMovementInput);
        }

        if (instantiated.TryGetComponent(out BaseInteractionInput baseInteractionInput))
        {
            interactionInputFocus.SetFocus(baseInteractionInput);
        }
        
        magicInputFocus.OnMagicInput(context);
    }
}


