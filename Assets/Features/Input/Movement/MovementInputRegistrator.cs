using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementInputRegistrator : MonoBehaviour
{
    [Header("Topic")]
    [SerializeField] private MovementInputFocus movementInputFocus;
    [SerializeField] private BaseMovementInput movementInputBehaviour;

    [Header("RegistrationTime")] 
    [SerializeField] private bool awake;
    [SerializeField] private bool start;

    private void Awake()
    {
        if (awake)
        {
            movementInputFocus.SetFocus(movementInputBehaviour);
        }
    }

    private void Start()
    {
        if (start)
        {
            movementInputFocus.SetFocus(movementInputBehaviour);
        }
    }
}
