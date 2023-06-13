using UnityEngine;

namespace Features.Character.Scripts.Interaction
{
    public class InteractionInputRegistrator : MonoBehaviour
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
}
