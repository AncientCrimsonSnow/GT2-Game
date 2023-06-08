using Features.TileSystem.CharacterBehaviours;
using UnityEngine;

public class MagicInputRegistrator : MonoBehaviour
{
    [Header("Topic")]
    [SerializeField] private MagicInputFocus magicInputFocus;
    [SerializeField] private BaseMagicInput magicInputBehaviour;

    [Header("RegistrationTime")] 
    [SerializeField] private bool awake;
    [SerializeField] private bool start;

    private void Awake()
    {
        if (awake)
        {
            magicInputFocus.SetFocus(magicInputBehaviour);
        }
    }

    private void Start()
    {
        if (start)
        {
            magicInputFocus.SetFocus(magicInputBehaviour);
        }
    }
}
