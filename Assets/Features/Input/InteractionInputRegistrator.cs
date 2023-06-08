using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInputregistrator : MonoBehaviour
{
    [Header("Topic")]
    [SerializeField] private InteractionInputFocus interactionInputFocus;
    [SerializeField] private BaseInteractionInput interactionInputBehaviour;

    [Header("RegistrationTime")] 
    [SerializeField] private bool awake;
    [SerializeField] private bool start;

    private void Awake()
    {
        if (awake)
        {
            interactionInputFocus.SetFocus(interactionInputBehaviour);
        }
    }

    private void Start()
    {
        if (start)
        {
            interactionInputFocus.SetFocus(interactionInputBehaviour);
        }
    }
}
